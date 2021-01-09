using System.Collections.Generic;
using Npgsql;

namespace SWE1_MTCG.DBFeature
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

        public List<object[]> QueryDatabase(INpgsqlCommand npgsqlCommand)
        {
            List<object[]> results = new List<object[]>();
            npgsqlCommand.Connection = _npgsqlConnection;
            INpgsqlDataReader resultReader;
            _npgsqlConnection.Open();
            resultReader = npgsqlCommand.ExecuteReader();
            while (resultReader.Read())
            {
              object[] row = new object[resultReader.FieldCount()];
              for (int i = 0; i < row.Length; i++)
              {
                  row[i] = resultReader.GetValue(i);
              }
              results.Add(row);
            }
            _npgsqlConnection.Close();
            return results;
        }
    }
}