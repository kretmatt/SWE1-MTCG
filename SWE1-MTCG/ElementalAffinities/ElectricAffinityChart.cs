using System.Collections.Generic;
using SWE1_MTCG.Enums;

namespace SWE1_MTCG.ElementalAffinities
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