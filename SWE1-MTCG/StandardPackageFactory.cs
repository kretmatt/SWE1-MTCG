using System.Collections.Generic;

namespace SWE1_MTCG
{
    public class StandardPackageFactory:IPackageFactory
    {
        /* Both packages are used for testing purposes (and current implementation) */
        public static List<ICard> standardPackage= new List<ICard>()
        {
            new Goblin("Goblin warrior", 5, new NormalElementalAttribute(), 1,2),
            new Elf("Fire elf ranger", 10, new FireElementalAttribute(), 5, 3),
            new SpellCard("Water gun", 5, new WaterElementalAttribute()),
            new SpellCard("Fireball", 15, new FireElementalAttribute()),
            new Wizzard("Merlin", 45,new NormalElementalAttribute(), 4,4)
        };
        
        public static List<ICard> highRarityPackage = new List<ICard>()
        {
            new AreaCard("Sunny battlefield", 2, new NormalElementalAttribute(), EAreaType.ELEMENTAL),
            new Knight("Gawain", 15,new FireElementalAttribute(), 20,9),
            new Goblin("Absurdly strong goblin", 10, new WaterElementalAttribute(), 2, 1000),
            new Dragon("Fafnir", 100,new FireElementalAttribute(), 20,7),
            new SpellCard("Aqua Sphere", 20, new WaterElementalAttribute())
        };
        
        public IPackage CreatePackage()
        {
            IPackage package = new StandardPackage();
            package.AddCardRange(standardPackage);
            return package;
        }

        public IPackage CreateHighRarityPackage()
        {
            IPackage package = new StandardPackage();
            package.AddCardRange(highRarityPackage);
            return package;
        }
    }
}