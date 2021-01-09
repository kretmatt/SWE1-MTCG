using System.Collections.Generic;

namespace SWE1_MTCG
{
    public class ElectricAffinityChart:AAffinityChart
    {
        public ElectricAffinityChart()
        {
            elementalStrengths=new List<EElementalType>
            {
                EElementalType.WATER
            };
            elementalWeaknesses=new List<EElementalType>
            {
                EElementalType.GROUND
            };
            ElementalType = EElementalType.ELECTRIC;
        }
    }
}