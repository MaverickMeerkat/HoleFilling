using HoleFilling.DataObjects;
using System;

namespace HoleFilling
{
    public static class DistanceMeasures
    {
        // Chebyshev distance
        public static int ChebyshevDistance(Pixel x, Pixel y)
        {
            return Math.Max(Math.Abs(x.X - y.X), Math.Abs(x.Y - y.Y));
        }
    }
}
