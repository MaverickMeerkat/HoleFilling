using HoleFilling.DataObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoleFilling
{
    public class SpiralTraverser : IEnumerable<Pixel>
    {
        private readonly ImageMatrix _image;

        // keep global variables for reuse 
        private int _x;
        private int _y;
        private int _upperBound;
        private int _rightBound;
        private int _bottomBound;
        private int _leftBound;
        private int _counter;

        public SpiralTraverser(Rectangle rectangle, ImageMatrix image)
        {
            _upperBound = rectangle.TopLeft.Xi;
            _leftBound = rectangle.TopLeft.Yi;
            _rightBound = rectangle.BottomRight.Yi;
            _bottomBound = rectangle.BottomRight.Xi;

            _x = _upperBound;
            _y = _leftBound;
            _counter = (_rightBound - _leftBound + 1) * (_bottomBound - _upperBound + 1) - 1;
            _image = image;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public IEnumerator<Pixel> GetEnumerator()
        {
            // use local variables to be restarted in the next foreach statement
            var upperBound = _upperBound;
            var bottomBound = _bottomBound;
            var leftBound = _leftBound;
            var rightBound = _rightBound;
            var counter = _counter;
            var x = _x;
            var y = _y;

            upperBound++; // initial increase of boundary so 

            while (counter > 0)
            {
                // go right until you hit boundary
                while(y < rightBound)
                {                    
                    yield return _image.GetArrayElement(x, y);
                    y++;
                    counter--;
                }

                rightBound--;

                // go down until you hit boundary
                while (x < bottomBound)
                {
                    yield return _image.GetArrayElement(x, y);
                    x++;
                    counter--;
                }

                bottomBound--;

                // go left until you hit boundary
                while (y > leftBound)
                {
                    yield return _image.GetArrayElement(x, y);
                    y--;
                    counter--;
                }

                leftBound++;

                // go up until you hit boundary
                while (x > upperBound)
                {
                    yield return _image.GetArrayElement(x, y);
                    x--;
                    counter--;
                }

                upperBound++;
            }

            yield return _image.GetArrayElement(x, y); // last element when all bounds are same...
        }


    }
}
