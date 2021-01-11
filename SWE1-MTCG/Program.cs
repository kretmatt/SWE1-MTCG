using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Json.Net;
using SWE1_MTCG.Battle;
using SWE1_MTCG.DBFeature;
using SWE1_MTCG.DTOs;
using SWE1_MTCG.Enums;
using SWE1_MTCG.WebService;

namespace SWE1_MTCG
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            IHTTPServer baseHttpServer = new BaseHTTPServer(10001);
            baseHttpServer.Start();

            /*ICardRepository cardRepository = new CardRepository();
            cardRepository.CreateServantCard("Arash", "Death doesn't even faze him anymore.", 30, EElementalType.GROUND,
                EServantClass.ARCHER);
            cardRepository.CreateServantCard("King Arthur", "The leader of the round table is not all talk.", 25,
                EElementalType.NORMAL, EServantClass.SABER);
            cardRepository.CreateServantCard("Heracles",
                "Although madness shrouds his mind, he should not be underestimated.", 30, EElementalType.NORMAL,
                EServantClass.BERSERKER);
            cardRepository.CreateServantCard("Li Shuwen", "Also known as God Spear Li.", 27, EElementalType.WATER,
                EServantClass.LANCER);
            cardRepository.CreateServantCard("Alexander the Great",
                "The king of conquerors might conquer this battlefield as well", 31, EElementalType.GROUND,
                EServantClass.RIDER);
            cardRepository.CreateServantCard("Medea", "Best known for her role in the myth of Jason and the Argonauts.",
                25, EElementalType.FIRE, EServantClass.CASTER);
            cardRepository.CreateServantCard("Sasaki Kojirou", "Why is he an assassin again?", 29,
                EElementalType.ELECTRIC, EServantClass.ASSASSIN);


            cardRepository.CreateSpellCard("Ray of frost",
                "A beam of frigid white-blue light streaks toward the enemy.", 20, EElementalType.WATER);
            cardRepository.CreateSpellCard("Eldritch Blast",
                "A beam of crackling energy streaks toward a creature within range.", 25, EElementalType.NORMAL);
            cardRepository.CreateSpellCard("Shocking grasp",
                "Lightning springs from your hand to deliver a shock to a creature you try to touch.", 27,
                EElementalType.ELECTRIC);
            cardRepository.CreateSpellCard("Poison spray",
                "You extend your hand toward a creature you can see within range and project a puff of noxious gas from your palm.",
                24, EElementalType.GROUND);
            cardRepository.CreateSpellCard("Fireball",
                "A bright streak flashes from your pointing finger to a point you choose within range and then blossoms with a low roar into an explosion of flame.",
                28, EElementalType.FIRE);


            cardRepository.CreateTrapCard("Pitfall trap", "How did you fall for that?", 15, EElementalType.GROUND, 10);
            cardRepository.CreateTrapCard("Glue trap", "It's pretty sticky!", 13, EElementalType.WATER, 9);

            cardRepository.CreateAreaCard("Volcano", "The battlefield transforms into the peak of an active volcano!",
                6, EElementalType.FIRE, 5);
            cardRepository.CreateAreaCard("Riverbank",
                "This peaceful riverbank will soon witness the horrors of battle", 5, EElementalType.WATER, 7);

            cardRepository.CreateMonsterCard("Deep sea kraken", "Somehow the intense pressure of the deep sea made it's skin impenetrable to magic spells", 34, EElementalType.WATER, EMonsterType.KRAKEN);
            cardRepository.CreateMonsterCard("Evil wizzard", "Wizzards are very dangerous, due to them having mastered the arcane arts.", 21,EElementalType.ELECTRIC,EMonsterType.WIZZARD);
            cardRepository.CreateMonsterCard("Fat ork", "Orcs are savage humanoids with stooped postures, piggish faces, and prominent teeth that resemble tusks",20, EElementalType.WATER, EMonsterType.ORK );
            cardRepository.CreateMonsterCard("Fire elf", "Fire elves have known dragons since their childhood and can evade their attacks.", 18, EElementalType.FIRE,EMonsterType.ELF);
            cardRepository.CreateMonsterCard("Goblin soldier", "Goblins are weak on their own, but you shouldn't underestimate them in groups.", 15, EElementalType.NORMAL,EMonsterType.GOBLIN);
            cardRepository.CreateMonsterCard("Fafnir", "The evil dragon, whose blood made Siegfried invincible.", 44,EElementalType.GROUND,EMonsterType.DRAGON);
            cardRepository.CreateMonsterCard("Knight A", "A random knight, who still needs to make a name for himself.", 22, EElementalType.GROUND, EMonsterType.KNIGHT);*/
        }
    }
}