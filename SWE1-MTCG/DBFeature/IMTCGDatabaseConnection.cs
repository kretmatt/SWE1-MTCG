using System.Collections.Generic;

namespace SWE1_MTCG.DBFeature
{
    public interface IMTCGDatabaseConnection
    {
        int ExecuteStatement(INpgsqlCommand npgsqlCommand);
        List<object[]> QueryDatabase(INpgsqlCommand npgsqlCommand);
    }
}