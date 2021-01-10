using System.Collections.Generic;
using SWE1_MTCG.Enums;

namespace SWE1_MTCG.ElementalAffinities
{
    public class GroundAffinityChart:AAffinityChart
    {
        public GroundAffinityChart()
        {
            elementalStrengths=new List<EElementalType>
            {
                EElementalType.ELECTRIC
            };
            elementalWeaknesses=new List<EElementalType>
            {
                EElementalType.WATER
            };
            ElementalType = EElementalType.GROUND;
        }
    }
}