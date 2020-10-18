using System.Collections.Generic;

namespace SWE1_MTCG
{
    public class WaterElementalAttribute:AElementalAttribute
    {
        public WaterElementalAttribute()
        {
            strengths = new List<EElementalAttributes>()
            {
                EElementalAttributes.FIRE
            };
            weaknesses = new List<EElementalAttributes>()
            {
                EElementalAttributes.NORMAL
            };
            type = EElementalAttributes.WATER;
        }
    }
}