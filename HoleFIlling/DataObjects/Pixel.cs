using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoleFilling.DataObjects
{
    /// <summary>
    /// A grayscale normalized representation of a pixel, containing value of [0,1], or -1 if holed
    /// </summary>
    public class Pixel
    {
        /// <summary>
        /// Row
        /// </summary>
        public int Xi { get; }

        /// <summary>
        /// Column
        /// </summary>
        public int Yi { get; }

        /// <summary>
        /// Normalized grayscale value of [0,1], or -1 if holed
        /// </summary>
        public float Value { get; set; }

        public Pixel(int x, int y, float val)
        {
            Xi = x;
            Yi = y;
            Value = val;
        }
    }
}
