using System;
using System.Collections.Generic;
using Json.Net;
namespace SWE1_MTCG
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            ICardRepository cardRepository = new CardRepository();
            //Console.WriteLine(JsonNet.Serialize(cardRepository.ReadAll()));
            IUserRepository userRepository = new UserRepository();
            ITradingDealRepository tradingDealRepository = new TradingDealRepository();
            //tradingDealRepository.ConductTrade(tradingDealRepository.Read(8), userRepository.Read(1));
            cardRepository.CreateMonsterCard("Goblin", "Just a random goblin!", 4.2, EElementalType.NORMAL,
                EMonsterType.GOBLIN);
        }
    }
}