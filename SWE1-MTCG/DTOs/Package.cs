using System.Collections.Generic;

namespace SWE1_MTCG.DTOs
{
    public class Package
    {
        public int Id { get; set; }
        public List<ACard> Cards { get; set; }
    }
}