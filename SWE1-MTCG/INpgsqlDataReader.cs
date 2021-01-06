namespace SWE1_MTCG
{
    public interface INpgsqlDataReader
    {
        bool Read();
        object GetValue(int i);
    }
}