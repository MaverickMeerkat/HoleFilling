using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoleFilling
{
    public class Pixel
    {
        public int Xi { get; private set; }
        public int Yi { get; private set; }
        public float Value { get; set; }

        public Pixel(int x, int y, float val)
        {
            Xi = x;
            Yi = y;
            Value = val;
        }
    }

    public interface IImageMatrix
    {
        int LenX { get; }
        int LenY { get; }

        bool IsHoled { get; set; }

        Pixel GetArrayElement(int x, int y);
    }

    public class ImageMatrix : IImageMatrix
    {
        private Pixel[,] _array { get; set; }

        public int LenX { get; }
        public int LenY { get; }

        public bool IsHoled { get; set; }

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

        public Pixel GetArrayElement(int x, int y)
        {
            return _array[x, y];
        }        
    }

    public class Hole
    {
        public List<Pixel> HolePixels { get; set; }
        public Rectangle CoveringRectangle { get; set; }

        public Hole()
        {
            HolePixels = new List<Pixel>();
        }
    }

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
