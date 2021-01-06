namespace SWE1_MTCG
{
    public interface INpgsqlCommand
    {
        int ExecuteNonQuery();
        INpgsqlDataReader ExecuteReader();

        Npgsql.NpgsqlParameterCollection Parameters { get;}
        
        Npgsql.NpgsqlConnection Connection { get; set; }
    }
}