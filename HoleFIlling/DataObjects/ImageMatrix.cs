using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoleFilling.DataObjects
{
    /// <summary>
    /// A grayscale normalized representation of an image as a matrix of pixels. 
    /// Each pixel containing a float value between [0,1], or -1 if holed.
    /// </summary>
    public class ImageMatrix
    {
        public int LenX { get; protected set; }
        public int LenY { get; protected set; }
        public bool IsHoled { get; set; }

        private Pixel[,] _array;

        public ImageMatrix(Pixel[,] array)
        {
            _array = array;
            LenX = array.GetLength(0);
            LenY = array.GetLength(1);
        }

        public ImageMatrix(float[,] array)
        {
            LenX = array.GetLength(0);
            LenY = array.GetLength(1);
            _array = new Pixel[LenX, LenY];

            for (int i = 0; i < LenX; i++)
                for (int j = 0; j < LenY; j++)
                    _array[i, j] = new Pixel(i, j, array[i, j]);
        }

        /// <summary>
        /// Retrieves a pixel element from the image matrix.
        /// </summary>
        /// <param name="x">row</param>
        /// <param name="y">column</param>
        /// <returns></returns>
        public Pixel GetArrayElement(int x, int y)
        {
            return _array[x, y];
        }


        /// <summary>
        /// Gets another pixel that is x,y distant from original element; Returns null if outside of scope
        /// </summary>
        /// <param name="start"></param>
        /// <param name="x">rows offset</param>
        /// <param name="y">collumns offset</param>
        /// <returns></returns>
        public Pixel GetDistantElement(Pixel start, int x, int y)
        {
            if (start.Xi + x > LenX || start.Xi + x < 0 ||
                start.Yi + y > LenY || start.Yi + y < 0)
                return null;

            return GetArrayElement(start.Xi + x, start.Yi + y);
        }
    }
}
