using System.Text;
using SWE1_MTCG.Enums;

namespace SWE1_MTCG.DTOs
{
    public abstract class ACard
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Damage { get; set; }
        public EElementalType ElementalType { get; set; }
        //CardType is set, when cards are loaded from the database (cardtype itself is not saved in the db)
        public ECardType CardType { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0} - {1} - {2} - {3} - {4} - {5}\n", Id, Name, Description, Damage,
                ElementalType.ToString(), CardType.ToString());
            return sb.ToString();
        }
    }
}