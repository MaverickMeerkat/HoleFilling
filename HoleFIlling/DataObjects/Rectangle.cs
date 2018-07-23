using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoleFilling.DataObjects
{
    /// <summary>
    /// Holds the 4 edge pixels of a rectangle
    /// </summary>
    public class Rectangle
    {
        public Pixel TopLeft { get; }
        public Pixel BottomRight { get; }
        public Pixel TopRight { get; }
        public Pixel BottomLeft { get; }

        public Rectangle(Pixel topLeft, Pixel bottomRight, Pixel topRight, Pixel bottomLeft)
        {
            TopLeft = topLeft;
            BottomRight = bottomRight;
            TopRight = topRight;
            BottomLeft = bottomLeft;
        }
    }
}
