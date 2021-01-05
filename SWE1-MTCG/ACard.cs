namespace SWE1_MTCG
{
    public abstract class ACard
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Damage { get; set; }
        public EElementalType ElementalType { get; set; }

        public ACard(string name, string description, int damage, EElementalType elementalType)
        {
            Name = name;
            Description = description;
            Damage = damage;
            ElementalType = elementalType;
        }
    }
}