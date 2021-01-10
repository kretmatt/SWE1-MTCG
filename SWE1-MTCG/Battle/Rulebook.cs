using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using SWE1_MTCG.DTOs;
using SWE1_MTCG.ElementalAffinities;
using SWE1_MTCG.Enums;
using SWE1_MTCG.MonsterFeature;
using SWE1_MTCG.ServantFeature;

namespace SWE1_MTCG.Battle
{
    public class Rulebook:IRulebook
    {
        private List<AServantHierarchy> servantHierarchies;
        private List<AAffinityChart> affinityCharts;
        private List<AMonsterHierarchy> monsterHierarchies;
        
        public Rulebook()
        {
            servantHierarchies = new List<AServantHierarchy>
            {
                new SaberHierarchy(),
                new ArcherHierarchy(),
                new LancerHierarchy(),
                new CasterHierarchy(),
                new AssassinHierarchy(),
                new RiderHierarchy(),
                new BerserkerHierarchy()
            };
            
            affinityCharts = new List<AAffinityChart>
            {
                new ElectricAffinityChart(),
                new FireAffinityChart(),
                new GroundAffinityChart(),
                new WaterAffinityChart(),
                new NormalAffinityChart()
            };
            
            monsterHierarchies = new List<AMonsterHierarchy>
            {
                new DragonHierarchy(),
                new ElfHierarchy(),
                new GoblinHierarchy(),
                new WizzardHierarchy(),
                new OrkHierarchy(),
                new KnightHierarchy(),
                new KrakenHierarchy()
            };
        }
        
        public BattleResult DetermineVictor(ACard attacker, ACard defender)
        {
            //attacker and defender are just names for the parameters; there is no special priority
            StringBuilder stringBuilder = new StringBuilder();
            double attackerDamage = attacker.Damage;
            double defenderDamage = defender.Damage;
            double defenderDamageModifier = affinityCharts
                .Single(ac => ac.ElementalType == defender.ElementalType)
                .CalculateElementalAttackModifier(attacker.ElementalType);
            double attackerDamageModifier = attackerDamageModifier = affinityCharts
                .Single(ac => ac.ElementalType == attacker.ElementalType)
                .CalculateElementalAttackModifier(defender.ElementalType);
            BattleResult br = new BattleResult();
            br.Attacker = attacker;
            br.Defender = defender;
            //only servant battle
            if (attacker.CardType == ECardType.SERVANT && defender.CardType == ECardType.SERVANT)
            {
                attackerDamageModifier *=
                    servantHierarchies.Single(sh => sh.ServantClass == ((ServantCard) defender).ServantClass)
                        .CalculateServantAttackModifier(((ServantCard) attacker).ServantClass);
                defenderDamageModifier *= servantHierarchies
                    .Single(sh => sh.ServantClass == ((ServantCard) attacker).ServantClass)
                    .CalculateServantAttackModifier(((ServantCard) defender).ServantClass);

                stringBuilder.AppendFormat("SERVANT only fight! {0} vs. {1}: ",attacker.Name,defender.Name);
                    //sb.Append("{0} and {1} fight! {0} attacks with {2} damage! {1} strikes back with {3} damage!",attacker.Name,defender.Name, firstParticipantDamage,secondParticipantDamage);
                br.DefenderDamage = defenderDamage*defenderDamageModifier;
                br.AttackerDamage = attackerDamage*attackerDamageModifier;

                stringBuilder.AppendFormat("{0} deals {1} damage and {2} counters with {3} damage!", attacker.Name, br.AttackerDamage, defender.Name, br.DefenderDamage);
                if (br.AttackerDamage > br.DefenderDamage)
                    br.Victor = attacker;
                else if (br.DefenderDamage > br.AttackerDamage)
                    br.Victor = defender;
                else
                    br.Victor = null;
                br.BattleDescription = stringBuilder.ToString();
                return br;
            }else if (attacker.CardType == ECardType.MONSTER && defender.CardType == ECardType.MONSTER)
            {
                //only monster battle 
                stringBuilder.AppendFormat("MONSTER only fight! {0} vs. {1}: ",attacker.Name,defender.Name);

                defenderDamageModifier = monsterHierarchies
                    .Single(mh => mh.MonsterType == ((MonsterCard) defender).MonsterType)
                    .CalculateMonsterAttackModifier(((MonsterCard) attacker).MonsterType, ((MonsterCard)attacker).ElementalType);
                
                attackerDamageModifier = monsterHierarchies
                    .Single(mh => mh.MonsterType == ((MonsterCard) attacker).MonsterType)
                    .CalculateMonsterAttackModifier(((MonsterCard) defender).MonsterType, ((MonsterCard)defender).ElementalType);
                br.BattleDescription = "Battle desc";
                //sb.Append("{0} and {1} fight! {0} attacks with {2} damage! {1} strikes back with {3} damage!",attacker.Name,defender.Name, firstParticipantDamage,secondParticipantDamage);
                br.DefenderDamage = defenderDamage*defenderDamageModifier;
                br.AttackerDamage = attackerDamage*attackerDamageModifier;
                stringBuilder.AppendFormat("{0} deals {1} damage and {2} counters with {3} damage!", attacker.Name, br.AttackerDamage, defender.Name, br.DefenderDamage);
                if (br.AttackerDamage > br.DefenderDamage)
                    br.Victor = attacker;
                else if (br.DefenderDamage > br.AttackerDamage)
                    br.Victor = defender;
                else
                    br.Victor = null;
                br.BattleDescription = stringBuilder.ToString();
                return br;
            }
            //mixed battle
            stringBuilder.AppendFormat("Mixed Battle! {0} vs. {1}: ",attacker.Name,defender.Name);

            if (attacker.CardType == ECardType.MONSTER)
            {
                defenderDamageModifier *= monsterHierarchies
                    .Single(mh => mh.MonsterType == ((MonsterCard) attacker).MonsterType)
                    .CalculateMixedBattleEnemyAttackModifier(defender.CardType, defender.ElementalType);
                attackerDamageModifier*=monsterHierarchies
                    .Single(mh => mh.MonsterType == ((MonsterCard) attacker).MonsterType)
                    .CalculateMixedBattleOwnAttackModifier(defender.CardType, defender.ElementalType);
            }
            else if (defender.CardType == ECardType.MONSTER)
            {
                defenderDamageModifier *= monsterHierarchies
                    .Single(mh => mh.MonsterType == ((MonsterCard) defender).MonsterType)
                    .CalculateMixedBattleOwnAttackModifier(attacker.CardType, attacker.ElementalType);
                attackerDamageModifier*=monsterHierarchies
                    .Single(mh => mh.MonsterType == ((MonsterCard) defender).MonsterType)
                    .CalculateMixedBattleEnemyAttackModifier(attacker.CardType, attacker.ElementalType);
            }
            //sb.Append("{0} and {1} fight! {0} attacks with {2} damage! {1} strikes back with {3} damage!",attacker.Name,defender.Name, firstParticipantDamage,secondParticipantDamage);
            br.DefenderDamage = defenderDamage*defenderDamageModifier;
            br.AttackerDamage = attackerDamage*attackerDamageModifier;
            stringBuilder.AppendFormat("{0} deals {1} damage and {2} counters with {3} damage!", attacker.Name, br.AttackerDamage, defender.Name, br.DefenderDamage);
            if (br.AttackerDamage > br.DefenderDamage)
                br.Victor = attacker;
            else if (br.DefenderDamage > br.AttackerDamage)
                br.Victor = defender;
            else
                br.Victor = null;
            br.BattleDescription = stringBuilder.ToString();
            return br;
        }
    }
}