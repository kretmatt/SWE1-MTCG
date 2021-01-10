using System.Collections.Generic;
using SWE1_MTCG.DTOs;
using SWE1_MTCG.Enums;

namespace SWE1_MTCG.DBFeature
{
    public interface IBattleHistoryRepository
    {
        int Create(User user, EBattleResult battleResult, int pointChange);
        BattleHistory Read(int id, User user);
        List<BattleHistory> ReadAll(User user);
    }
}