namespace SWE1_MTCG
{
    public class TrapCard:ACard
    {
        public int Uses { get; set; }
        
        public TrapCard(string name, string description, int damage, EElementalType elementalType, int uses) : base(name, description, damage, elementalType)
        {
            Uses = uses;
        }
    }
}