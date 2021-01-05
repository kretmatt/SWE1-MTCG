namespace SWE1_MTCG
{
    public class MonsterCard:ACard
    {
        public EMonsterType MonsterType { get; set; }
        
        public MonsterCard(string name, string description, int damage, EElementalType elementalType, EMonsterType monsterType) : base(name, description, damage, elementalType)
        {
            MonsterType = monsterType;
        }
    }
}