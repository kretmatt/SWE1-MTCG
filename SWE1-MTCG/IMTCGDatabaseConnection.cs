using System.Collections.Generic;
using Npgsql;

namespace SWE1_MTCG
{
    public interface IMTCGDatabaseConnection
    {
        int ExecuteStatement(INpgsqlCommand npgsqlCommand);
        List<object[]> QueryDatabase(INpgsqlCommand npgsqlCommand);
    }
}