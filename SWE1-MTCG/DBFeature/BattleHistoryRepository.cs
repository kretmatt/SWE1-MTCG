using System;
using System.Collections.Generic;
using SWE1_MTCG.DTOs;
using SWE1_MTCG.Enums;

namespace SWE1_MTCG.DBFeature
{
    public class BattleHistoryRepository:IBattleHistoryRepository
    {
        private IMTCGDatabaseConnection _mtcgDatabaseConnection;
        private IUserRepository _userRepository;
        
        public BattleHistoryRepository()
        {
            _mtcgDatabaseConnection=MTCGDatabaseConnection.ReturnMTCGDatabaseConnection();
            _userRepository=new UserRepository();
            
        }

        public BattleHistory ConvertToBattleHistory(object[] row, User user)
        {
            BattleHistory battleHistory = new BattleHistory();

            battleHistory.Id = Convert.ToInt32(row[0]);
            battleHistory.User = user;
            EBattleResult battleResult;
            if(EBattleResult.TryParse(row[2].ToString(), out battleResult))
                battleHistory.BattleResult = battleResult;
            battleHistory.PointChange = Convert.ToInt32(row[3]);

            return battleHistory;
        }
        
        public int Create(User user, EBattleResult battleResult, int pointChange)
        {
            INpgsqlCommand checkUserCommand = new NpsqlCommand("SELECT * FROM mtcguser WHERE id=@id;");
            checkUserCommand.Parameters.AddWithValue("id", user.Id);
            List<object[]> checkUserResults = _mtcgDatabaseConnection.QueryDatabase(checkUserCommand);

            if (checkUserResults.Count != 1)
                return 0;
            
            INpgsqlCommand createBattleHistoryCommand = new NpsqlCommand("INSERT INTO battlehistory (userid, battleresult, pointchange) VALUES (@userid, @battleresult, @pointchange)");
            createBattleHistoryCommand.Parameters.AddWithValue("userid", user.Id);
            createBattleHistoryCommand.Parameters.AddWithValue("battleresult", battleResult.ToString());
            createBattleHistoryCommand.Parameters.AddWithValue("pointchange", pointChange);

            int result = _mtcgDatabaseConnection.ExecuteStatement(createBattleHistoryCommand);

            return result;
        }

        public BattleHistory Read(int id, User user)
        {
            INpgsqlCommand readBattleHistoryCommand = new NpsqlCommand("SELECT * FROM battlehistory WHERE id=@id AND userid=@userid;");
            readBattleHistoryCommand.Parameters.AddWithValue("id", id);
            readBattleHistoryCommand.Parameters.AddWithValue("userid", user.Id);
            List<object[]> readBattleHistoryResults = _mtcgDatabaseConnection.QueryDatabase(readBattleHistoryCommand);

            if (readBattleHistoryResults.Count != 1)
                return null;

            return ConvertToBattleHistory(readBattleHistoryResults[0], user);
        }

        public List<BattleHistory> ReadAll(User user)
        {
            INpgsqlCommand readAllBattleHistoriesCommand = new NpsqlCommand("SELECT * FROM battlehistory WHERE userid=@userid;");
            readAllBattleHistoriesCommand.Parameters.AddWithValue("userid", user.Id);
            List<object[]> readAllBattleHistoriesResults =
                _mtcgDatabaseConnection.QueryDatabase(readAllBattleHistoriesCommand);
            List<BattleHistory> battleHistories = new List<BattleHistory>();

            foreach (object[] row in readAllBattleHistoriesResults)
            {
                battleHistories.Add(ConvertToBattleHistory(row,user));
            }

            return battleHistories;
        }
    }
}