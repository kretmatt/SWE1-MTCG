using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SWE1_MTCG.DTOs;
using SWE1_MTCG.Enums;

namespace SWE1_MTCG.Battle
{
    public class BattleSystem
    {
        private IRulebook rulebook;
        private static ConcurrentQueue<User> userQueue;
        private IArena _arena;
        private object padlock = new object();
        public bool HasStarted = false;
        private Task<BattleSummary> GetBattleSummary;
        private BattleSummary _battleSummary;
        Random _random = new Random();
        
        public BattleSystem()
        {
            rulebook=new Rulebook();
            userQueue = new ConcurrentQueue<User>();
            _arena=new Arena();
        }
        
        public async Task<BattleSummary> DuelEnqueue(User user)
        {
            userQueue.Enqueue(user);
            
            lock (padlock)
            {
                if (GetBattleSummary == null || GetBattleSummary.IsCompleted)
                {
                    if (!HasStarted)
                    {
                        while(userQueue.Count<2){}
                        User attacker = new User();
                        User defender = new User();
                        userQueue.TryDequeue(out attacker);
                        userQueue.TryDequeue(out defender);
                        GetBattleSummary = Duel(attacker, defender);
                        HasStarted = true;
                    }
                }
            }
                
            _battleSummary = await GetBattleSummary;
            HasStarted = false;
            return _battleSummary;
        }

        public async Task<BattleSummary> Duel(User attacker, User defender)
        {
            BattleSummary bs = new BattleSummary();
            bs.BattleResults=new List<BattleResult>();
            int roundCount = 0;
            //wincounter -> if attacker wins +1, if defender wins -1; negative number at the end = defender won; positive number at the end = attacker won; 0 = draw
            int winCounter = 0;
            while (roundCount != 100 && attacker.CardDeck.Count != 0 && defender.CardDeck.Count != 0)
            {
                roundCount += 1;

                int attackerIndex = _random.Next(attacker.CardDeck.Count);
                int defenderIndex = _random.Next(defender.CardDeck.Count);

                ACard attackCard = attacker.CardDeck[attackerIndex];
                ACard defendCard = defender.CardDeck[defenderIndex];

                BattleResult battleResult = _arena.ConductBattle(attackCard, defendCard, rulebook);
                
                bs.BattleResults.Add(battleResult);

                if (defendCard.CardType == ECardType.AREA)
                {
                    ((AreaCard) defendCard).Uses -= 1;
                    if (((AreaCard) defendCard).Uses <= 0)
                    {
                        defender.CardDeck.Remove(defendCard);
                    }
                }
                else if (defendCard.CardType == ECardType.TRAP)
                {
                    ((TrapCard) defendCard).Uses -= 1;
                    if (((TrapCard) defendCard).Uses <= 0)
                    {
                        defender.CardDeck.Remove(defendCard);
                    }
                }
                if (attackCard.CardType == ECardType.AREA)
                {
                    ((AreaCard) attackCard).Uses -= 1;
                    if (((AreaCard) attackCard).Uses <= 0)
                    {
                        attacker.CardDeck.Remove(attackCard);
                    }
                }
                else if (attackCard.CardType == ECardType.TRAP)
                {
                    ((TrapCard) attackCard).Uses -= 1;
                    if (((TrapCard) attackCard).Uses <= 0)
                    {
                        attacker.CardDeck.Remove(attackCard);
                    }
                }
                
                if (battleResult.Victor == attackCard)
                {
                    winCounter += 1;
                    if (defender.CardDeck.Contains(defendCard))
                    {
                        defender.CardDeck.Remove(defendCard);
                        attacker.CardDeck.Add(defendCard);
                    }
                }else if (battleResult.Victor == defendCard)
                {
                    winCounter -= 1;
                    if (attacker.CardDeck.Contains(attackCard))
                    {
                        attacker.CardDeck.Remove(attackCard);
                        defender.CardDeck.Add(attackCard);
                    }
                }
            }

            if (winCounter > 0)
            {
                bs.Victor = attacker;
                bs.Loser = defender;
            }
            else if (winCounter < 0)
            {
                bs.Victor = defender;
                bs.Loser = attacker;
            }
            else
            {
                bs.Victor = null;
                bs.Loser = null;
            }

            bs.BattleStats = winCounter;
            _arena=new Arena();
            return bs;
        }

    }
}