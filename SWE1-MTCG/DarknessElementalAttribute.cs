using System.Collections.Generic;

namespace SWE1_MTCG
{
    public class DarknessElementalAttribute:AElementalAttribute
    {
        public DarknessElementalAttribute()
        {
            strengths = new List<EElementalAttributes>()
            {
                EElementalAttributes.FIRE
            };
            weaknesses = new List<EElementalAttributes>()
            {
                EElementalAttributes.LIGHT
            };
            type = EElementalAttributes.DARKNESS;
        }
    }
}