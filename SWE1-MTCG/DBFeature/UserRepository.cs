using System;
using System.Collections.Generic;
using System.Linq;
using SWE1_MTCG.DTOs;

namespace SWE1_MTCG.DBFeature
{
    public class UserRepository:IUserRepository
    {
        private IMTCGDatabaseConnection _mtcgDatabaseConnection;
        private ICardRepository _cardRepository;
        
        public UserRepository()
        {
            _mtcgDatabaseConnection = MTCGDatabaseConnection.ReturnMTCGDatabaseConnection();
            _cardRepository = new CardRepository();
        }

        private User ConvertToUser(object[] row)
        {
            User user = new User();
            user.Id = Int32.Parse(row[0].ToString());
            user.Username = row[1].ToString();
            user.Bio = row[2].ToString();
            user.Coins = Int32.Parse(row[4].ToString());
            user.CardDeck = _cardRepository.LoadCardDeck(user);
            user.CardStack = _cardRepository.LoadCardStack(user);
            return user;
        }

        private bool CheckUniquenessOfUsername(string username)
        {
            INpgsqlCommand checkUsernameCommand = new NpsqlCommand("SELECT * FROM mtcguser WHERE username = @username;");
            checkUsernameCommand.Parameters.AddWithValue("username", username);
            if (_mtcgDatabaseConnection.QueryDatabase(checkUsernameCommand).Count > 0)
                return false;
            return true;
        }
        
        public int Create(string username, string password, string bio)
        {
            if (!CheckUniquenessOfUsername(username))
                return 0;
            
            int coins = 20;
            INpgsqlCommand createUserCommand = new NpsqlCommand("INSERT INTO mtcguser (username, bio, password, coins) VALUES (@username, @bio, @password, @coins)");
            createUserCommand.Parameters.AddWithValue("username", username);
            createUserCommand.Parameters.AddWithValue("password", password);
            createUserCommand.Parameters.AddWithValue("bio", bio);
            createUserCommand.Parameters.AddWithValue("coins", coins);
            int createUserResult = _mtcgDatabaseConnection.ExecuteStatement(createUserCommand);
            if (createUserResult != 1)
                return createUserResult;
            
            User createdUser = Read(username);

            if (createdUser == null)
                return 0;
            
            INpgsqlCommand createUserStatsCommand = new NpsqlCommand("INSERT INTO userstats (id, points, winloseratio) VALUES (@id,@startpoints, @startwinloseratio)");
            createUserStatsCommand.Parameters.AddWithValue("startpoints", 1000);
            createUserStatsCommand.Parameters.AddWithValue("startwinloseratio", 0);
            createUserStatsCommand.Parameters.AddWithValue("id", createdUser.Id);
            int createUserStatsResult = _mtcgDatabaseConnection.ExecuteStatement(createUserStatsCommand);
            if (createUserStatsResult != 1)
                return createUserStatsResult;
            return createUserResult + createUserStatsResult;
        }

        public int Update(User user, string username, string bio)
        {
            if (!CheckUniquenessOfUsername(username))
                return 0;
            
            INpgsqlCommand npgsqlUpdateCommand = new NpsqlCommand("UPDATE mtcguser SET username = @username, bio = @bio WHERE id = @id;");
            npgsqlUpdateCommand.Parameters.AddWithValue("username", username);
            npgsqlUpdateCommand.Parameters.AddWithValue("bio", bio);
            npgsqlUpdateCommand.Parameters.AddWithValue("id", user.Id);
            return _mtcgDatabaseConnection.ExecuteStatement(npgsqlUpdateCommand);
        }

        public int Delete(string username)
        {
            INpgsqlCommand npgsqlDeleteWithUsernameCommand = new NpsqlCommand("DELETE FROM mtcguser WHERE username = @username;");
            npgsqlDeleteWithUsernameCommand.Parameters.AddWithValue("username", username);
            return _mtcgDatabaseConnection.ExecuteStatement(npgsqlDeleteWithUsernameCommand);
        }

        public int Delete(int id)
        {
            INpgsqlCommand npgsqlDeleteWithIdCommand = new NpsqlCommand("DELETE FROM mtcguser WHERE id = @id;");
            npgsqlDeleteWithIdCommand.Parameters.AddWithValue("id", id);
            return _mtcgDatabaseConnection.ExecuteStatement(npgsqlDeleteWithIdCommand);
        }

        public User Read(int id)
        {
            INpgsqlCommand readUserWithIdCommand =
                new NpsqlCommand("Select * FROM mtcguser WHERE id = @id;");
            readUserWithIdCommand.Parameters.AddWithValue("id", id);
            List<object[]> results = _mtcgDatabaseConnection.QueryDatabase(readUserWithIdCommand);
            if (results.Count != 1)
                return null;
            
            return ConvertToUser(results[0]);
        }

        public User Read(string username)
        {
            INpgsqlCommand readUserWithUsernameCommand =
                new NpsqlCommand("Select * FROM mtcguser WHERE username = @username;");
            readUserWithUsernameCommand.Parameters.AddWithValue("username", username);
            List<object[]> results = _mtcgDatabaseConnection.QueryDatabase(readUserWithUsernameCommand);

            if (results.Count != 1)
                return null;
            
            return ConvertToUser(results[0]);
        }

        public int SetCardDeck(List<ACard> cards, User user)
        {
            if (Read(user.Id) == null || cards.Count!=4)
                return 0;
            List<ACard> userCardStack = _cardRepository.LoadCardStack(user);
            foreach (ACard card in cards)
            {
                INpgsqlCommand npgsqlCommand = new NpsqlCommand("SELECT * FROM tradingdeal WHERE userid = @userid AND cardid = @cardid;");
                npgsqlCommand.Parameters.AddWithValue("userid", user.Id);
                npgsqlCommand.Parameters.AddWithValue("cardid", card.Id);

                int tradeOffers = _mtcgDatabaseConnection.QueryDatabase(npgsqlCommand).Count;
                if ((tradeOffers>0 &&
                     (userCardStack.Count(c => c.Id == card.Id) < 2)) ||
                    (!userCardStack.Exists(c => c.Id == card.Id)) ||
                    (userCardStack.Count(c => c.Id == card.Id) < cards.Count(c => c.Id == card.Id)))
                    return 0;
            }
            
            INpgsqlCommand clearCardDeckOfUserCommand = new NpsqlCommand("DELETE FROM carddeck WHERE userid = @userid;");
            clearCardDeckOfUserCommand.Parameters.AddWithValue("userid", user.Id);
            int affectedrows = _mtcgDatabaseConnection.ExecuteStatement(clearCardDeckOfUserCommand);
            
            foreach (ACard card in cards)
            {
                INpgsqlCommand addCardToUsersCardDeckCommand = new NpsqlCommand("INSERT INTO carddeck (userid,cardid) VALUES (@userid,@cardid);");
                addCardToUsersCardDeckCommand.Parameters.AddWithValue("userid", user.Id);
                addCardToUsersCardDeckCommand.Parameters.AddWithValue("cardid", card.Id);
                affectedrows += _mtcgDatabaseConnection.ExecuteStatement(addCardToUsersCardDeckCommand);
            }
            
            return affectedrows;
        }

        public int DeductCoins(int coinAmount, User user)
        {
            if (!CoinsDeductible(coinAmount,user))
                return 0;

            int newCoinAmount = user.Coins - coinAmount;
            
            INpgsqlCommand updateUserCoinsCommand = new NpsqlCommand("UPDATE mtcguser SET coins = @coins WHERE id = @id;");
            updateUserCoinsCommand.Parameters.AddWithValue("coins", newCoinAmount);
            updateUserCoinsCommand.Parameters.AddWithValue("id", user.Id);

            return _mtcgDatabaseConnection.ExecuteStatement(updateUserCoinsCommand);
        }

        public bool CoinsDeductible(int coinAmount, User user)
        {
            user = Read(user.Id);
            return user!=null && coinAmount>=0 && user.Coins>=coinAmount;
        }
    }
}