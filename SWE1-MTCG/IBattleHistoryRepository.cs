using System.Collections.Generic;

namespace SWE1_MTCG
{
    public interface IBattleHistoryRepository
    {
        int Create(User user, EBattleResult battleResult, int pointChange);
        BattleHistory Read(int id, User user);
        List<BattleHistory> ReadAll(User user);
    }
}