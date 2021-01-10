using System.Collections.Generic;
using NUnit.Framework;
using SWE1_MTCG.Battle;
using SWE1_MTCG.DTOs;
using SWE1_MTCG.Enums;

namespace SWE1_MTCG_TestProject
{
    [TestFixture]
    public class BattleTests
    {
        private List<ACard> mockCards;
        private Arena _arena;
        private IRulebook _rulebook;

        [SetUp]
        public void SetUp()
        {
            _arena= new Arena();
            _rulebook=new Rulebook();
            mockCards = new List<ACard>
            {
                new ServantCard()
                {
                    Id = 1,
                    CardType = ECardType.SERVANT,
                    Damage = 20,
                    Description = "Death doesn't even faze him anymore.",
                    ElementalType = EElementalType.GROUND,
                    Name = "Arash",
                    ServantClass = EServantClass.ARCHER
                },
                new ServantCard{
                    Id = 2,
                    CardType = ECardType.SERVANT,
                    Damage = 10,
                    Description = "He needs no second strike!",
                    ElementalType = EElementalType.NORMAL,
                    Name = "Li Shuwen",
                    ServantClass = EServantClass.LANCER
                },
                new MonsterCard
                {
                    CardType = ECardType.MONSTER,
                    Damage = 1,
                    Description = "Fire elves have known dragons since their childhood.",
                    ElementalType = EElementalType.FIRE,
                    Id = 3,
                    MonsterType = EMonsterType.ELF,
                    Name = "Fire elf"
                },
                new MonsterCard{
                    CardType = ECardType.MONSTER,
                    Damage = 10000,
                    Description = "It's the evil dragon, whose blood made Siegfried invincible.",
                    ElementalType = EElementalType.GROUND,
                    Id = 4,
                    MonsterType = EMonsterType.DRAGON,
                    Name = "Fafnir"
                },new SpellCard
                {
                    CardType = ECardType.SPELL,
                    Damage = 10,
                    Description = "Taken from the pokemon games.",
                    ElementalType = EElementalType.WATER,
                    Id = 5,
                    Name = "Aqua Sphere"
                },
                new MonsterCard{
                    CardType = ECardType.MONSTER,
                    Damage = 100,
                    Description = "A no-name knight.",
                    ElementalType = EElementalType.FIRE,
                    Id = 6,
                    MonsterType = EMonsterType.KNIGHT,
                    Name = "Knight A"
                },new TrapCard
                {
                    Id=7,
                    CardType = ECardType.TRAP,
                    Damage = 10,
                    Description = "How did you fall for that?",
                    ElementalType = EElementalType.GROUND,
                    Name = "Pitfall trap",
                    Uses = 10
                },new AreaCard
                {
                    Id=8,
                    CardType = ECardType.AREA,
                    Damage = 5,
                    Description = "Look it's a volcano!",
                    ElementalType = EElementalType.FIRE,
                    Name = "Volcano",
                    Uses = 5
                }
                
            };
        }
        [Test]
        public void ConductBattleTest()
        {
            //arrange - Arrange expected victors for some example fights
            ACard servantBattleWinner = mockCards[1];
            ACard dragonFireElfBattleWinner = mockCards[2];
            ACard trapAreaBattleWinner = mockCards[6];
            ACard knightWaterSpellBattleWinner = mockCards[4];
            //act
            ACard servantBattleResult = _arena.ConductBattle(mockCards[0], mockCards[1], _rulebook).Victor;
            ACard dragonFireElfBattleResult = _arena.ConductBattle(mockCards[2], mockCards[3], _rulebook).Victor;
            ACard trapAreaBattleResult = _arena.ConductBattle(mockCards[6], mockCards[7], _rulebook).Victor;
            ACard knightWaterSpellBattleResult = _arena.ConductBattle(mockCards[4], mockCards[5], _rulebook).Victor;
            //assert
            Assert.AreEqual(servantBattleWinner,servantBattleResult);
            Assert.AreEqual(dragonFireElfBattleWinner,dragonFireElfBattleResult);
            Assert.AreEqual(trapAreaBattleWinner,trapAreaBattleResult);
            Assert.AreEqual(knightWaterSpellBattleWinner,knightWaterSpellBattleResult);
            //Trap was set and used in the fight afterwards
            Assert.IsNull(_arena.attackerTrap);
            //Once the area is set, it won't disappear until it is changed
            Assert.IsNotNull(_arena.battleFieldArea);
        }
    }
}