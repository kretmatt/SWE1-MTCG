using System;
using System.Collections.Generic;
using Npgsql.Replication.PgOutput.Messages;

namespace SWE1_MTCG
{
    public class SessionRepository:ISessionRepository
    {
        private IMTCGDatabaseConnection _mtcgDatabaseConnection;
        private IUserRepository _userRepository;
        //Session time in minutes
        public static int SessionTime = 5;
        public SessionRepository()
        {
            _mtcgDatabaseConnection = MTCGDatabaseConnection.ReturnMTCGDatabaseConnection();
            _userRepository=new UserRepository();
        }
        
        public bool Login(string username, string password)
        {
            INpgsqlCommand loginCommand = new NpsqlCommand("Select password FROM mtcguser WHERE username=@username;");
            loginCommand.Parameters.AddWithValue("username", username);
            List<object[]> loginResults = _mtcgDatabaseConnection.QueryDatabase(loginCommand);
            if (loginResults.Count != 1)
                return false;
            //Compare passwords
            if (loginResults[0][0].ToString() != password)
                return false;
            //Check if user is logged in; if so update time, if not create session 
            User user = _userRepository.Read(username);
            if (user == null)
                return false;
            INpgsqlCommand checkIfSessionExists = new NpsqlCommand("SELECT * FROM currentsessions WHERE id=@id;");
            checkIfSessionExists.Parameters.AddWithValue("id", user.Id);
            List<object[]> checkIfSessionExistsResults = _mtcgDatabaseConnection.QueryDatabase(checkIfSessionExists);

            if (checkIfSessionExistsResults.Count == 0)
            {
                INpgsqlCommand createSessionCommand = new NpsqlCommand("INSERT INTO currentsessions(id) VALUES (@id);");
                createSessionCommand.Parameters.AddWithValue("id", user.Id);
                int createSessionResult = _mtcgDatabaseConnection.ExecuteStatement(createSessionCommand);
                if (createSessionResult != 1)
                    return false;
            }
            else
            {
                INpgsqlCommand updateSessionCommand = new NpsqlCommand("UPDATE currentsessions SET logintime=CURRENT_TIMESTAMP WHERE id=@id;");
                updateSessionCommand.Parameters.AddWithValue("id", user.Id);
                int updateSessionResult = _mtcgDatabaseConnection.ExecuteStatement(updateSessionCommand);
                if (updateSessionResult != 1)
                    return false;
            }
            return true;
        }

        public bool CheckIfInValidSession(string username)
        {
            User user = _userRepository.Read(username);
            if (user == null)
                return false;
            
            INpgsqlCommand selectSessionCommand = new NpsqlCommand("SELECT * FROM currentsessions WHERE id=@id;");
            selectSessionCommand.Parameters.AddWithValue("id", user.Id);
            List<object[]> selectSessionResults = _mtcgDatabaseConnection.QueryDatabase(selectSessionCommand);
            
            if (selectSessionResults.Count == 0 || selectSessionResults.Count>1)
                return false;
            
            DateTime currTime = DateTime.Now;
            DateTime loginTime = Convert.ToDateTime(selectSessionResults[0][1]);
            loginTime=loginTime.AddMinutes(SessionTime);

            if (loginTime < currTime)
            {
                Logout(username);
                return false;
            }
                
            return true;
        }

        public bool Logout(string username)
        {
            User user = _userRepository.Read(username);
            if (user == null)
                return false;
            INpgsqlCommand logoutCommand = new NpsqlCommand("DELETE FROM currentsessions WHERE id=@id;");
            logoutCommand.Parameters.AddWithValue("id", user.Id);
            int logoutResult = _mtcgDatabaseConnection.ExecuteStatement(logoutCommand);
            if (logoutResult < 1)
                return false;
            return true;
        }
    }
}