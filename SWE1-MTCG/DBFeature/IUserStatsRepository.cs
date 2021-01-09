using System.Collections.Generic;
using SWE1_MTCG.DTOs;

namespace SWE1_MTCG.DBFeature
{
    public interface IUserStatsRepository
    {
        List<UserStats> ReadAll();
        UserStats Read(User user);
        int Update(User user);
    }
}