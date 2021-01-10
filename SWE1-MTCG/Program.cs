using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Json.Net;
using SWE1_MTCG.Battle;
using SWE1_MTCG.DBFeature;
using SWE1_MTCG.DTOs;
using SWE1_MTCG.Enums;

namespace SWE1_MTCG
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            ServantCard arash = new ServantCard()
            {
                Id = 1,
                CardType = ECardType.SERVANT,
                Damage = 20,
                Description = "aas",
                ElementalType = EElementalType.GROUND,
                Name = "Arash",
                ServantClass = EServantClass.ARCHER
            };
            
            ServantCard lishuwn = new ServantCard{
                Id = 2,
                CardType = ECardType.SERVANT,
                Damage = 10,
                Description = "aas",
                ElementalType = EElementalType.NORMAL,
                Name = "Li Shuwen",
                ServantClass = EServantClass.LANCER
            };
            
            MonsterCard fireelf = new MonsterCard
            {
                CardType = ECardType.MONSTER,
                Damage = 1,
                Description = "HEy",
                ElementalType = EElementalType.FIRE,
                Id = 3,
                MonsterType = EMonsterType.ELF,
                Name = "Fire elf"
            };
            
            MonsterCard dragon = new MonsterCard{
                CardType = ECardType.MONSTER,
                Damage = 10000,
                Description = "Die",
                ElementalType = EElementalType.NORMAL,
                Id = 4,
                MonsterType = EMonsterType.DRAGON,
                Name = "Dragon"
            };

            SpellCard sp = new SpellCard
            {
                CardType = ECardType.SPELL,
                Damage = 10,
                Description = "hey",
                ElementalType = EElementalType.WATER,
                Id = 5,
                Name = "Wasserpistole"
            };
            
            MonsterCard knight = new MonsterCard{
                CardType = ECardType.MONSTER,
                Damage = 10000,
                Description = "Die",
                ElementalType = EElementalType.FIRE,
                Id = 4,
                MonsterType = EMonsterType.KNIGHT,
                Name = "Strong knight"
            };

            AreaCard beach = new AreaCard
            {
                Id=5,
                CardType = ECardType.AREA,
                Damage = 5,
                Description = "Look it's the beach!",
                ElementalType = EElementalType.WATER,
                Name = "Beach",
                Uses = 5
            };
            
            AreaCard volcano = new AreaCard
            {
                Id=6,
                CardType = ECardType.AREA,
                Damage = 5,
                Description = "Look it's a volcano!",
                ElementalType = EElementalType.FIRE,
                Name = "Volcano",
                Uses = 5
            };

            TrapCard pitfall = new TrapCard
            {
                Id=7,
                CardType = ECardType.TRAP,
                Damage = 10,
                Description = "How did you fall for that?",
                ElementalType = EElementalType.GROUND,
                Name = "Pitfall trap",
                Uses = 10
            };
            List<ACard> cards = new List<ACard>
            {
                arash,
                lishuwn,
                fireelf,
                dragon,
                sp,
                knight,
                beach,
                volcano,
                pitfall
            };

            User attacker = new User { 
                Username = "Franz",
                CardDeck = new List<ACard>
                {
                    lishuwn,
                    volcano,
                    sp,
                    fireelf
                }};
            User defender = new User
            {
                Username = "Fritz",
                CardDeck = new List<ACard>
                {
                    arash,
                    beach,
                    knight,
                    dragon
                }
            };
            
            BattleSystem bs = new BattleSystem();
            
            Thread t1 = new Thread(async delegate()
            {
                Console.WriteLine("Thread 1");
                BattleSummary bsum = await bs.DuelEnqueue(attacker);
                Console.WriteLine("Victor:{0}",bsum.Victor.Username);
                Console.WriteLine("Battlestats:{0}",bsum.BattleStats);
                Console.WriteLine("Thread 1");
            });
            t1.Start();
            
            Thread t2 = new Thread(async delegate()
            {
                Console.WriteLine("Thread 2");
                BattleSummary bsum = await bs.DuelEnqueue(defender);
                Console.WriteLine("Victor:{0}",bsum.Victor.Username);
                Console.WriteLine("Battlestats:{0}",bsum.BattleStats);
                Console.WriteLine("Thread 2");
            });
            t2.Start();
            
            
            
            Console.WriteLine("Let's see the result!");
        }
    }
}