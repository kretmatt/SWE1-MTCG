namespace SWE1_MTCG
{
    public class NpgsqlDataReader:INpgsqlDataReader
    {
        private Npgsql.NpgsqlDataReader _npgsqlDataReader;
        
        public NpgsqlDataReader(Npgsql.NpgsqlDataReader npgsqlDataReader)
        {
            _npgsqlDataReader = npgsqlDataReader;
        }

        public bool Read() => _npgsqlDataReader.Read();
        public object GetValue(int i) => _npgsqlDataReader.GetValue(i);
    }
}