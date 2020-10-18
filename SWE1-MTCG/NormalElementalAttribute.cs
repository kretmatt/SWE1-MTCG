using System.Collections.Generic;

namespace SWE1_MTCG
{
    public class NormalElementalAttribute:AElementalAttribute
    {
        public NormalElementalAttribute()
        {
            strengths = new List<EElementalAttributes>()
            {
                EElementalAttributes.WATER
            };
            weaknesses = new List<EElementalAttributes>()
            {
                EElementalAttributes.FIRE
            };
            type = EElementalAttributes.NORMAL;
        }
    }
}