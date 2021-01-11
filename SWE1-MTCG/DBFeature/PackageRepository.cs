using System;
using System.Collections.Generic;
using SWE1_MTCG.DTOs;

namespace SWE1_MTCG.DBFeature
{
    public class PackageRepository:IPackageRepository
    {
        private IMTCGDatabaseConnection _mtcgDatabaseConnection;
        private ICardRepository _cardRepository;
        private IUserRepository _userRepository;
        private static int packageCosts = 5;
        public PackageRepository()
        {
            _mtcgDatabaseConnection = MTCGDatabaseConnection.ReturnMTCGDatabaseConnection();
            _cardRepository = new CardRepository();
            _userRepository = new UserRepository();
        }
        
        public int CreatePackage(List<ACard> cards)
        {
            if (cards.Count != 5)
                return 0;

            foreach (ACard card in cards)
            {
                if (_cardRepository.Read(card.Id) == null)
                    return 0;
            }
            
            INpgsqlCommand createPackageCommand = new NpsqlCommand("INSERT INTO package DEFAULT VALUES;");
            int affectedRows = _mtcgDatabaseConnection.ExecuteStatement(createPackageCommand);
            //Wasn't really sure how to retrieve the id of the newly created package. ExecuteStatement returns the amount of affected rows -> useless in this scenario. Package-Table only has the id field (id has auto increment), due to that it's pretty difficult to retrieve the new id. I settled for max(id) although it's not a good solution (as soon as some random big number is inserted there will be problems creating packages)
            INpgsqlCommand selectNewestIdCommand = new NpsqlCommand("SELECT max(id) FROM package;");
            List<object[]> selectNewestIdResults = _mtcgDatabaseConnection.QueryDatabase(selectNewestIdCommand);

            if (selectNewestIdResults.Count != 1)
                return affectedRows;

            int packageId = Convert.ToInt32(selectNewestIdResults[0][0]);
            
            foreach (ACard card in cards)
            {
                INpgsqlCommand addCardToPackageCommand = new NpsqlCommand("INSERT INTO packagecards (packageid, cardid) VALUES (@packageid, @cardid);");
                addCardToPackageCommand.Parameters.AddWithValue("packageid",packageId);
                addCardToPackageCommand.Parameters.AddWithValue("cardid", card.Id);
                affectedRows += _mtcgDatabaseConnection.ExecuteStatement(addCardToPackageCommand);
            }
            return affectedRows;
        }

        public int OpenPackage(User user)
        {
            if (!_userRepository.CoinsDeductible(packageCosts, user))
                return 0;
            // Same problem as above, just with the "oldest" package instead.

            INpgsqlCommand readAllPackagesCommand = new NpsqlCommand("SELECT min(id) FROM package;");
            List<object[]> readAllPackagesResults = _mtcgDatabaseConnection.QueryDatabase(readAllPackagesCommand);

            if (readAllPackagesResults.Count != 1)
                return 0;
            //FIFO - First in, first out principle for packages
            //Try catch was added after the submission of the project
            int packageId = 0;
            try
            {
                packageId = Convert.ToInt32(readAllPackagesResults[0][0]);
            }
            catch (Exception e)
            {
                return 0;
            }
            packageId = Convert.ToInt32(readAllPackagesResults[0][0]);
            INpgsqlCommand readPackageCardsCommand = new NpsqlCommand("SELECT * FROM packagecards WHERE packageid = @packageid;");
            readPackageCardsCommand.Parameters.AddWithValue("packageid", packageId);

            List<object[]> readPackageCardsResults = _mtcgDatabaseConnection.QueryDatabase(readPackageCardsCommand);
            int affectedrows = 0;

            foreach (object[] row in readPackageCardsResults)
            {
                INpgsqlCommand addToUserCardStackCommand = new NpsqlCommand("INSERT INTO cardstack (userid, cardid) VALUES (@userid,@cardid);");
                addToUserCardStackCommand.Parameters.AddWithValue("userid", user.Id);
                addToUserCardStackCommand.Parameters.AddWithValue("cardid",Convert.ToInt32(row[2]));
                affectedrows += _mtcgDatabaseConnection.ExecuteStatement(addToUserCardStackCommand);
            }

            affectedrows += _userRepository.DeductCoins(packageCosts, user);
            affectedrows += DeletePackage(packageId);
            return affectedrows;
        }

        public int DeletePackage(int id)
        {
            INpgsqlCommand deletePackageCommand = new NpsqlCommand("DELETE FROM PACKAGE WHERE id = @id;");
            deletePackageCommand.Parameters.AddWithValue("id", id);
            return _mtcgDatabaseConnection.ExecuteStatement(deletePackageCommand);
        }
    }
}