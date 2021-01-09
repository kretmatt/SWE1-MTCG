namespace SWE1_MTCG.DBFeature
{
    public interface INpgsqlCommand
    {
        int ExecuteNonQuery();
        INpgsqlDataReader ExecuteReader();

        Npgsql.NpgsqlParameterCollection Parameters { get;}
        
        Npgsql.NpgsqlConnection Connection { get; set; }
    }
}