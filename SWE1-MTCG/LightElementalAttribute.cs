using System.Collections.Generic;

namespace SWE1_MTCG
{
    public class LightElementalAttribute:AElementalAttribute
    {
        public LightElementalAttribute()
        {
            strengths = new List<EElementalAttributes>()
            {
                EElementalAttributes.WATER
            };
            weaknesses = new List<EElementalAttributes>()
            {
                EElementalAttributes.DARKNESS
            };
            type = EElementalAttributes.LIGHT;
        }
    }
}