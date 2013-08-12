using System.Collections.Generic;

namespace vrp
{
    public class VrpResult
    {
        public double Dist { get; set; }

        public List<int>[] Routes { get; set; } 
    }
}
