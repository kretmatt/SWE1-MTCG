namespace SWE1_MTCG
{
    public class AreaCard:ACard
    {
        public int Uses { get; set; }
        
        public AreaCard(string name, string description, int damage, EElementalType elementalType, int uses) : base(name, description, damage, elementalType)
        {
            Uses = uses;
        }
    }
}