using System.CodeDom;
using Npgsql;

namespace SWE1_MTCG
{
    public class NpsqlCommand:INpgsqlCommand
    {
        private Npgsql.NpgsqlCommand _npgsqlCommand;

        public int ExecuteNonQuery() =>  Connection!=null?_npgsqlCommand.ExecuteNonQuery():0;

        public INpgsqlDataReader ExecuteReader()=>Connection !=null?new NpgsqlDataReader(_npgsqlCommand.ExecuteReader()):null;

        public NpgsqlParameterCollection Parameters => _npgsqlCommand.Parameters;

        public NpgsqlConnection Connection
        {
            get
            {
                return _npgsqlCommand.Connection;
            }
            set
            {
                _npgsqlCommand.Connection = value;
            }
        }

        public NpsqlCommand(string cmdText)
        {
            _npgsqlCommand=new NpgsqlCommand(cmdText);
        }
    }
}