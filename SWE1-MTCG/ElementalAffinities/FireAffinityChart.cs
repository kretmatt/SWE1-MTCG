using System.Collections.Generic;
using SWE1_MTCG.Enums;

namespace SWE1_MTCG.ElementalAffinities
{
    public class FireAffinityChart:AAffinityChart
    {
        public FireAffinityChart()
        {
            elementalStrengths=new List<EElementalType>
            {
                EElementalType.NORMAL
            };
            elementalWeaknesses=new List<EElementalType>
            {
                EElementalType.WATER
            };
            ElementalType = EElementalType.FIRE;
        }
    }
}