using System.Collections.Generic;
using SWE1_MTCG.Enums;

namespace SWE1_MTCG.ElementalAffinities
{
    public class NormalAffinityChart:AAffinityChart
    {
        public NormalAffinityChart()
        {
            elementalStrengths=new List<EElementalType>
            {
                EElementalType.WATER
            };
            elementalWeaknesses=new List<EElementalType>
            {
                EElementalType.FIRE
            };
            ElementalType = EElementalType.NORMAL;
        }
    }
}