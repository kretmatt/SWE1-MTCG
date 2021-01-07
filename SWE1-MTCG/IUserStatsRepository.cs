using System.Collections.Generic;

namespace SWE1_MTCG
{
    public interface IUserStatsRepository
    {
        List<UserStats> ReadAll();
        UserStats Read(User user);
        int Update(User user);
    }
}