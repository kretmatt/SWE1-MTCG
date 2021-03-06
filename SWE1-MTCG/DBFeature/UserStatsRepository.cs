using System;
using System.Collections.Generic;
using SWE1_MTCG.DTOs;
using SWE1_MTCG.Enums;

namespace SWE1_MTCG.DBFeature
{
    public class UserStatsRepository:IUserStatsRepository
    {
        private IUserRepository _userRepository;
        private IMTCGDatabaseConnection _mtcgDatabaseConnection;
        private static int points = 1000;
        public UserStatsRepository()
        {
            _userRepository=new UserRepository();
            _mtcgDatabaseConnection = MTCGDatabaseConnection.ReturnMTCGDatabaseConnection();
        }

        private UserStats ConvertToUserStats(object[] row)
        {
            UserStats userStats = new UserStats();
            userStats.User = _userRepository.Read(Convert.ToInt32(row[0]));
            userStats.Points = Convert.ToInt32(row[1]);
            userStats.WinLoseRatio = Convert.ToDouble(row[2]);
            return userStats;
        }
        
        public List<UserStats> ReadAll()
        {
            INpgsqlCommand readAllUserStatsCommand = new NpsqlCommand("SELECt * FROM userstats;");
            List<object[]> readAllUserStatsResult = _mtcgDatabaseConnection.QueryDatabase(readAllUserStatsCommand);
            List<UserStats> allUserStats = new List<UserStats>();

            foreach (object[] row in readAllUserStatsResult)
            {
                allUserStats.Add(ConvertToUserStats(row));
            }

            return allUserStats;
        }

        public UserStats Read(User user)
        {
            if (_userRepository.Read(user.Username)==null)
                return null;
            
            INpgsqlCommand readUserStatsCommand = new NpsqlCommand("SELECT * FROM userstats WHERE id = @id;");
            readUserStatsCommand.Parameters.AddWithValue("id", user.Id);
            List<object[]> readUserStatsResults = _mtcgDatabaseConnection.QueryDatabase(readUserStatsCommand);

            if (readUserStatsResults.Count != 1)
                return null;

            return ConvertToUserStats(readUserStatsResults[0]);
        }

        public int Update(User user)
        {
            if (_userRepository.Read(user.Id)==null)
                return 0;
            
            INpgsqlCommand selectUserStatsFromBattleHistoryCommand = new NpsqlCommand("SELECT sum(pointchange), (SELECT count(*) FROM battlehistory WHERE battleresult = @win AND userid = @userid GROUP BY battleresult), (SELECT count(*) FROM battlehistory WHERE battleresult = @loss AND userid = @userid GROUP BY battleresult) FROM battlehistory WHERE userid = @userid GROUP BY userid;");
            selectUserStatsFromBattleHistoryCommand.Parameters.AddWithValue("userid", user.Id);
            selectUserStatsFromBattleHistoryCommand.Parameters.AddWithValue("win", EBattleResult.WIN.ToString());
            selectUserStatsFromBattleHistoryCommand.Parameters.AddWithValue("loss", EBattleResult.LOSS.ToString());
            
            List<object[]> selectUserStatsFromBattleHistory =
                _mtcgDatabaseConnection.QueryDatabase(selectUserStatsFromBattleHistoryCommand);

            if (selectUserStatsFromBattleHistory.Count != 1)
                return 0;

            int pointchange = 0;
            int wins = 0;
            int losses = 0;
            try
            {
                pointchange = Convert.ToInt32(selectUserStatsFromBattleHistory[0][0]);
            }
            catch (Exception e)
            {
                pointchange = 0;
            }

            try
            {
                wins = Convert.ToInt32(selectUserStatsFromBattleHistory[0][1]);
            }
            catch (Exception e)
            {
                wins = 0;
            }

            try
            {
                losses = Convert.ToInt32(selectUserStatsFromBattleHistory[0][2]);
            }
            catch (Exception e)
            {
                losses = 0;
            }

            double winloseratio;
            int currentPoints = points + pointchange;
            if (currentPoints < 0)
                currentPoints = 0;
            // win lose ratio = wins/losses; if user never lost, set win/lose ratio to amount of wins to avoid division by zero
            if (losses == 0)
                winloseratio = wins;
            else
                winloseratio = (double)wins / (double)losses;

            INpgsqlCommand updateUserStatsCommand = new NpsqlCommand("UPDATE userstats SET points = @points, winloseratio = @winloseratio WHERE id = @id;");
            updateUserStatsCommand.Parameters.AddWithValue("points", currentPoints);
            updateUserStatsCommand.Parameters.AddWithValue("winloseratio", winloseratio);
            updateUserStatsCommand.Parameters.AddWithValue("id", user.Id);
            int result = _mtcgDatabaseConnection.ExecuteStatement(updateUserStatsCommand);
            return result;
        }
    }
}