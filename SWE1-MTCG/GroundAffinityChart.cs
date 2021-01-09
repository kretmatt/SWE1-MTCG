using System.Collections.Generic;

namespace SWE1_MTCG
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