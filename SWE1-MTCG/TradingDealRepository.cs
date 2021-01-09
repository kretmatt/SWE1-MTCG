using System;
using System.Collections.Generic;
using System.Linq;
using Npgsql.Replication.PgOutput.Messages;

namespace SWE1_MTCG
{
    public class TradingDealRepository:ITradingDealRepository
    {
        private MTCGDatabaseConnection _mtcgDatabaseConnection;
        private IUserRepository _userRepository;
        private ICardRepository _cardRepository;
        public TradingDealRepository()
        {
            _mtcgDatabaseConnection=MTCGDatabaseConnection.ReturnMTCGDatabaseConnection();
            _userRepository = new UserRepository();
            _cardRepository = new CardRepository();
        }

        private TradingDeal ConvertToTradingDeal(object[] row)
        {
            TradingDeal tradingDeal = new TradingDeal();
            tradingDeal.Id = Convert.ToInt32(row[0]);
            tradingDeal.OfferedCard = _cardRepository.Read(Convert.ToInt32(row[1]));
            tradingDeal.OfferingUser = _userRepository.Read(Convert.ToInt32(row[2]));
            tradingDeal.RequiredCoins = Convert.ToInt32(row[3]);
            tradingDeal.MinDamage = Convert.ToInt32(row[4]);
            tradingDeal.CardType = (ECardType) Convert.ToInt32(row[5]);
            return tradingDeal;
        }
        
        public List<TradingDeal> ReadAll()
        {
            List<TradingDeal> tradingDeals = new List<TradingDeal>();
            INpgsqlCommand readAllTradingDealsCommand = new NpsqlCommand("SELECT * FROM tradingdeal;");
            List<object[]> readAllTradingDealsResults =
                _mtcgDatabaseConnection.QueryDatabase(readAllTradingDealsCommand);

            foreach (object[] row in readAllTradingDealsResults)
            {
                TradingDeal tradingDeal = ConvertToTradingDeal(row);
                tradingDeals.Add(tradingDeal);
            }

            return tradingDeals;
        }

        public TradingDeal Read(int id)
        {
            INpgsqlCommand readTradingDealWithSpecifiedIdCommand = new NpsqlCommand("SELECT * FROM tradingdeal WHERE id=@id;");
            readTradingDealWithSpecifiedIdCommand.Parameters.AddWithValue("id", id);
            List<object[]> readTradingDealWithSpecifiedIdResults =
                _mtcgDatabaseConnection.QueryDatabase(readTradingDealWithSpecifiedIdCommand);

            if (readTradingDealWithSpecifiedIdResults.Count != 1)
                return null;

            return ConvertToTradingDeal(readTradingDealWithSpecifiedIdResults[0]);
        }

        public int Create(ACard offeredCard, int requiredCoins, int minDamage, ECardType wantedCardType, User user)
        {
            if ((_cardRepository.LoadCardDeck(user).Exists(c => c.Id == offeredCard.Id) &&
                 _cardRepository.LoadCardStack(user).Count(c => c.Id == offeredCard.Id) <=
                 _cardRepository.LoadCardDeck(user).Count(c => c.Id == offeredCard.Id)) ||
                !_cardRepository.LoadCardStack(user).Exists(c => c.Id == offeredCard.Id) ||
                _userRepository.Read(user.Id) == null || _cardRepository.Read(offeredCard.Id) == null || requiredCoins<0)
                return 0;

            INpgsqlCommand createTradingDealCommand = new NpsqlCommand("INSERT INTO tradingdeal (cardid,userid,requiredcoins,mindamage,tradingdealtype) VALUES (@cardid,@userid,@requiredcoins,@mindamage,@tradingdealtype);");
            createTradingDealCommand.Parameters.AddWithValue("cardid", offeredCard.Id);
            createTradingDealCommand.Parameters.AddWithValue("userid", user.Id);
            createTradingDealCommand.Parameters.AddWithValue("requiredcoins",requiredCoins);
            createTradingDealCommand.Parameters.AddWithValue("mindamage", minDamage);
            createTradingDealCommand.Parameters.AddWithValue("tradingdealtype", (int)wantedCardType);
            int affectedRows = _mtcgDatabaseConnection.ExecuteStatement(createTradingDealCommand);
            return affectedRows;
        }

        public int Delete(int id, User user)
        {
            INpgsqlCommand deleteTradingDealOfUserCommand = new NpsqlCommand("DELETE FROM tradingdeal WHERE id=@id AND userid=@userid;");
            deleteTradingDealOfUserCommand.Parameters.AddWithValue("id", id);
            deleteTradingDealOfUserCommand.Parameters.AddWithValue("userid", user.Id);
            return _mtcgDatabaseConnection.ExecuteStatement(deleteTradingDealOfUserCommand);
        }

        public int ConductTrade(TradingDeal tradingDeal, User user, ACard card)
        {
            if (!TradePossible(tradingDeal, user, card))
                return 0;

            INpgsqlCommand readCardsToBePossiblyDeletedFromVendorCommand = new NpsqlCommand("SELECT cs.id FROM cardstack cs LEFT JOIN carddeck c on cs.cardid = c.cardid AND cs.userid=c.userid WHERE cs.userid=@userid AND cs.cardid=@cardid AND c.id IS NULL;");
            readCardsToBePossiblyDeletedFromVendorCommand.Parameters.AddWithValue("userid", tradingDeal.OfferingUser.Id);
            readCardsToBePossiblyDeletedFromVendorCommand.Parameters.AddWithValue("cardid", tradingDeal.OfferedCard.Id);
            List<object[]> readCardsToBePossiblyDeletedFromVendorResults =
                _mtcgDatabaseConnection.QueryDatabase(readCardsToBePossiblyDeletedFromVendorCommand);

            if (readCardsToBePossiblyDeletedFromVendorResults.Count < 1)
                return 0;
            //Remove card from vendor cardstack
            INpgsqlCommand removeCardFromVendorCommand = new NpsqlCommand("DELETE FROM cardstack WHERE id=@id;");
            removeCardFromVendorCommand.Parameters.AddWithValue("id",Convert.ToInt32(readCardsToBePossiblyDeletedFromVendorResults[0][0]));
            int affectedRows = _mtcgDatabaseConnection.ExecuteStatement(removeCardFromVendorCommand);
            
            //Insert card into buyer cardstack
            INpgsqlCommand transferCardToTradePartnerCommand = new NpsqlCommand("INSERT INTO cardstack (userid,cardid) VALUES (@userid,@cardid);");
            transferCardToTradePartnerCommand.Parameters.AddWithValue("userid",user.Id);
            transferCardToTradePartnerCommand.Parameters.AddWithValue("cardid", tradingDeal.OfferedCard.Id);
            
            affectedRows += _mtcgDatabaseConnection.ExecuteStatement(transferCardToTradePartnerCommand);
            
            INpgsqlCommand readCardsToBePossiblyDeletedFromBuyerCommand = new NpsqlCommand("SELECT cs.id FROM cardstack cs LEFT JOIN carddeck c on cs.cardid = c.cardid AND cs.userid=c.userid WHERE cs.userid=@userid AND cs.cardid=@cardid AND c.id IS NULL;");
            readCardsToBePossiblyDeletedFromBuyerCommand.Parameters.AddWithValue("userid", user.Id);
            readCardsToBePossiblyDeletedFromBuyerCommand.Parameters.AddWithValue("cardid", card.Id);
            List<object[]> readCardsToBePossiblyDeletedFromBuyerResults =
                _mtcgDatabaseConnection.QueryDatabase(readCardsToBePossiblyDeletedFromBuyerCommand);

            if (readCardsToBePossiblyDeletedFromBuyerResults.Count < 1)
                return affectedRows;

            INpgsqlCommand removeCardFromBuyerCommand = new NpsqlCommand("DELETE FROM cardstack WHERE id=@id;");
            removeCardFromBuyerCommand.Parameters.AddWithValue("id",Convert.ToInt32(readCardsToBePossiblyDeletedFromBuyerResults[0][0]));
            affectedRows += _mtcgDatabaseConnection.ExecuteStatement(removeCardFromBuyerCommand);
            
            INpgsqlCommand transferCardToVendorCommand = new NpsqlCommand("INSERT INTO cardstack (userid,cardid) VALUES (@userid,@cardid);");
            transferCardToVendorCommand.Parameters.AddWithValue("userid",tradingDeal.OfferingUser.Id);
            transferCardToVendorCommand.Parameters.AddWithValue("cardid", card.Id);
            
            affectedRows += _mtcgDatabaseConnection.ExecuteStatement(transferCardToVendorCommand);
            
            List<TradingDeal> allTradingDealsFromUserForOfferedCard = ReadAll().Where(td=>td.OfferingUser.Id==tradingDeal.OfferingUser.Id&&td.OfferedCard.Id==tradingDeal.OfferedCard.Id).ToList();

            foreach (TradingDeal td in allTradingDealsFromUserForOfferedCard)
            {
                affectedRows += Delete(td.Id, td.OfferingUser);
            }
            
            return affectedRows;
        }

        public int ConductTrade(TradingDeal tradingDeal, User user)
        {
            if (!TradePossible(tradingDeal, user))
                return 0;

            INpgsqlCommand readCardsToBePossiblyDeletedCommand = new NpsqlCommand("SELECT cs.id FROM cardstack cs LEFT JOIN carddeck c on cs.cardid = c.cardid AND cs.userid=c.userid WHERE cs.userid=@userid AND cs.cardid=@cardid AND c.id IS NULL;");
            readCardsToBePossiblyDeletedCommand.Parameters.AddWithValue("userid", tradingDeal.OfferingUser.Id);
            readCardsToBePossiblyDeletedCommand.Parameters.AddWithValue("cardid", tradingDeal.OfferedCard.Id);
            List<object[]> readCardsToBePossiblyDeletedResults =
                _mtcgDatabaseConnection.QueryDatabase(readCardsToBePossiblyDeletedCommand);

            if (readCardsToBePossiblyDeletedResults.Count < 1)
                return 0;
            
            INpgsqlCommand removeCardFromVendorCommand = new NpsqlCommand("DELETE FROM cardstack WHERE id=@id;");
            removeCardFromVendorCommand.Parameters.AddWithValue("id",Convert.ToInt32(readCardsToBePossiblyDeletedResults[0][0]));
            int affectedRows = _mtcgDatabaseConnection.ExecuteStatement(removeCardFromVendorCommand);

            INpgsqlCommand transferCardToBuyerCommand = new NpsqlCommand("INSERT INTO cardstack (userid,cardid) VALUES (@userid,@cardid);");
            transferCardToBuyerCommand.Parameters.AddWithValue("userid",user.Id);
            transferCardToBuyerCommand.Parameters.AddWithValue("cardid", tradingDeal.OfferedCard.Id);
            
            affectedRows += _mtcgDatabaseConnection.ExecuteStatement(transferCardToBuyerCommand);
            affectedRows+=_userRepository.DeductCoins(tradingDeal.RequiredCoins, user);

            List<TradingDeal> allTradingDealsFromUserForOfferedCard = ReadAll().Where(td=>td.OfferingUser.Id==tradingDeal.OfferingUser.Id&&td.OfferedCard.Id==tradingDeal.OfferedCard.Id).ToList();

            foreach (TradingDeal td in allTradingDealsFromUserForOfferedCard)
            {
                affectedRows += Delete(td.Id, td.OfferingUser);
            }
            
            return affectedRows;
        }

        // check if trade is possible (card traded for card)
        public bool TradePossible(TradingDeal tradingDeal, User user, ACard card)
        {
            if (Read(tradingDeal.Id) == null || _userRepository.Read(user.Id) == null ||
                _cardRepository.Read(card.Id) == null || _cardRepository.LoadCardDeck(user).Exists(c=>c.Id==card.Id)||!_cardRepository.LoadCardStack(user).Exists(c=>c.Id==card.Id)||tradingDeal.OfferingUser.Id==user.Id)
                return false;

            if (card.Damage >= tradingDeal.MinDamage && card.CardType == tradingDeal.CardType)
                return true;
            return false;
        }
        // check if trade is possible (card traded for coins)
        public bool TradePossible(TradingDeal tradingDeal, User user)
        {
            if (_userRepository.Read(user.Id) == null||Read(tradingDeal.Id)==null||tradingDeal.OfferingUser.Id==user.Id)
                return false;

            return _userRepository.CoinsDeductible(tradingDeal.RequiredCoins, user);
        }
    }
}