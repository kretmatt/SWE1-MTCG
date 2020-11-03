using System;

namespace SWE1_MTCG
{
    public class MonsterCard:ICard
    {
        public string Name { get; set; }
        public double BaseDamage { get; set; }
        public IElementalAttribute ElementalAttribute { get; set; }        
        
        public double ArmorPoints { get; set; }
        
        public double Strength { get; set; }

        public MonsterCard()
        {
                
        }
        public MonsterCard(string name, double baseDamage, IElementalAttribute elementalAttribute, double armorPoints, double strength)
        {
            Name = name;
            BaseDamage = baseDamage;
            ElementalAttribute = elementalAttribute;
            ArmorPoints = armorPoints;
            Strength = strength;
        }
        
        public string PrintCard()
        {

            throw new System.NotImplementedException();
        }

        public virtual double ReceiveDamage(ICardAction cardAction)
        {
            double receivedDamage = cardAction.GetDamage();
            if (cardAction.Attacker().GetType() != typeof(MonsterCard))
                receivedDamage *= ElementalAttribute.CheckEffectiveness(cardAction.GetElementalAttribute());
            return receivedDamage-ArmorPoints>=0 ? receivedDamage-ArmorPoints:0;
        }

        public ICardAction UseCard()
        {
            return new AttackAction(BaseDamage*Strength,ElementalAttribute.GetElementalAttribute(),this);
        }
    }
}