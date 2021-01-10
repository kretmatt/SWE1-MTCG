using System.Collections.Generic;
using SWE1_MTCG.DTOs;

namespace SWE1_MTCG.Battle
{
    public class BattleSummary
    {public int BattleStats { get; set; }
        public User Victor { get; set; }
        public User Loser { get; set; }
        
        public List<BattleResult> BattleResults { get; set; }
    }
}