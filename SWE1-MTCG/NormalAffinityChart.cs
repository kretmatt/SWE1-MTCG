using System.Collections.Generic;

namespace SWE1_MTCG
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