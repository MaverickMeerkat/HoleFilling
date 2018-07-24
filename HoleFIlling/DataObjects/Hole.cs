using System.Collections.Generic;

namespace HoleFilling.DataObjects
{
    public class Hole
    {
        /// <summary>
        /// A list of pixels that consists the boundary of the hole
        /// </summary>
        public List<Pixel> Boundary { get; }

        /// <summary>
        /// A list of the actual hole-pixels
        /// </summary>
        public List<Pixel> HolePixels { get; }

        /// <summary>
        /// A rectangle object that consists of the 4 edge pixels that cover the hole
        /// </summary>
        public Rectangle CoveringRectangle { get; }

        public Hole(List<Pixel> boundary, List<Pixel> holePixels, Rectangle coveringRectangle)
        {
            Boundary = boundary;
            HolePixels = holePixels;
            CoveringRectangle = coveringRectangle;
        }
    }
}
