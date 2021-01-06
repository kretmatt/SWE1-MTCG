using System;
using System.Threading;
using Npgsql;

namespace SWE1_MTCG
{
    public class MTCGDatabaseConnection:IMTCGDatabaseConnection
    {
        private static MTCGDatabaseConnection _mtcgDatabaseConnection;
        private NpgsqlConnection _npgsqlConnection;
        private MTCGDatabaseConnection()
        {
            _npgsqlConnection=new NpgsqlConnection("Host=localhost;Port=5432;Username=mtcg;Password=mtcg;Database=mtcg;");
        }
        
        public static MTCGDatabaseConnection ReturnMTCGDatabaseConnection()
        {
            if (_mtcgDatabaseConnection == null)
            {
                _mtcgDatabaseConnection=new MTCGDatabaseConnection();
            }

            return _mtcgDatabaseConnection;
        }
        
        public int ExecuteStatement(INpgsqlCommand npgsqlCommand)
        {
            npgsqlCommand.Connection = _npgsqlConnection;
            _npgsqlConnection.Open();
            int affectedRows = npgsqlCommand.ExecuteNonQuery();
            _npgsqlConnection.Close();
            return affectedRows;
        }

        public INpgsqlDataReader QueryDatabase(INpgsqlCommand npgsqlCommand)
        {
            npgsqlCommand.Connection = _npgsqlConnection;
            INpgsqlDataReader resultReader;
            _npgsqlConnection.Open();
            resultReader = npgsqlCommand.ExecuteReader();
            _npgsqlConnection.Close();
            return resultReader;
        }
    }
}