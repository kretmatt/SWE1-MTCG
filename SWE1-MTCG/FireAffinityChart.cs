using System.Collections.Generic;

namespace SWE1_MTCG
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