using System.Collections.Generic;
using SWE1_MTCG.Enums;

namespace SWE1_MTCG.ElementalAffinities
{
    public class WaterAffinityChart:AAffinityChart
    {
        public WaterAffinityChart()
        {
            elementalStrengths=new List<EElementalType>
            {
                EElementalType.FIRE,
                EElementalType.GROUND
            };
            elementalWeaknesses=new List<EElementalType>
            {
                EElementalType.ELECTRIC,
                EElementalType.NORMAL
            };
            ElementalType = EElementalType.WATER;
        }
    }
}