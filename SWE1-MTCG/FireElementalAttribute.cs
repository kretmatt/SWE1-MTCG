using System.Collections.Generic;

namespace SWE1_MTCG
{
    public class FireElementalAttribute:AElementalAttribute
    {
        public FireElementalAttribute()
        {
            strengths = new List<EElementalAttributes>()
            {
                EElementalAttributes.NORMAL
            };
            weaknesses = new List<EElementalAttributes>()
            {
                EElementalAttributes.WATER
            };
            type = EElementalAttributes.FIRE;
        }
    }
}