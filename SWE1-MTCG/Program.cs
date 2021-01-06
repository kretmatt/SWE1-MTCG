using System;

namespace SWE1_MTCG
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            MTCGDatabaseConnection conn = MTCGDatabaseConnection.ReturnMTCGDatabaseConnection();
            INpgsqlCommand cmd = new NpsqlCommand("INSERT INTO card (name,description,damage,elementaltype) VALUES (@name, @description, @damage,@elementaltype);");
            cmd.Parameters.AddWithValue("name", "Enkidu");
            cmd.Parameters.AddWithValue("description", "The friend of the king of heroes.");
            cmd.Parameters.AddWithValue("damage", 100);
            cmd.Parameters.AddWithValue("elementaltype", EElementalType.NORMAL.ToString());
            Console.WriteLine(conn.ExecuteStatement(cmd));
        }
    }
}