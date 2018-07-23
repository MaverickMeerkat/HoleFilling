using HoleFilling.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoleFilling
{
    public static class DistanceMeasures
    {
        // Chebyshev distance
        public static int ChebyshevDistance(Pixel x, Pixel y)
        {
            return Math.Max(Math.Abs(x.Xi - y.Xi), Math.Abs(x.Yi - y.Yi));
        }
    }
}
