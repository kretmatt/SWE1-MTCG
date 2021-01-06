using Npgsql;

namespace SWE1_MTCG
{
    public interface IMTCGDatabaseConnection
    {
        int ExecuteStatement(INpgsqlCommand npgsqlCommand);
        INpgsqlDataReader QueryDatabase(INpgsqlCommand npgsqlCommand);
    }
}