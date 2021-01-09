using System.Collections.Generic;

namespace SWE1_MTCG
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