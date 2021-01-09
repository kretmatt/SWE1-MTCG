using System.Collections.Generic;
using SWE1_MTCG.DTOs;
using SWE1_MTCG.Enums;

namespace SWE1_MTCG.DBFeature
{
    public interface ITradingDealRepository
    {
        List<TradingDeal> ReadAll();
        TradingDeal Read(int id);
        int Create(ACard offeredCard, int requiredCoins, int minDamage, ECardType wantedCardType, User user);
        int Delete(int id, User user);
        int ConductTrade(TradingDeal tradingDeal, User user, ACard card);
        int ConductTrade(TradingDeal tradingDeal, User user);
        bool TradePossible(TradingDeal tradingDeal, User user, ACard card);
        bool TradePossible(TradingDeal tradingDeal, User user);
    }
}