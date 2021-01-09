namespace SWE1_MTCG.DBFeature
{
    public interface INpgsqlDataReader
    {
        bool Read();
        object GetValue(int i);

        int FieldCount();
    }
}