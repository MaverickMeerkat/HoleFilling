using System.Collections.Generic;

namespace HoleFilling
{
    public class MooreTrace : ITraceAlgorithm
    {
        public List<Pixel> Trace(IImageMatrix img)
        {
            Pixel hole_pixel;
            for (int i = 0; i < img.LenX; i++)
                for (int j = 0; j < img.LenY; j++)
                {
                    hole_pixel = img.GetArrayElement(i, j);
                    if (hole_pixel.Value == -1)                                                               
                        // while goto statements are not highly approved of, as they create hard-to-read spaghetti code, 
                        // this (breaking out of nested loops) is one of the rare cases where they should be used, 
                        // as they are more coherent than setting up multiple flags, or using sub-methods that will return. 
                        goto Hole_Exists;                    
                }

            // if here - no hole was found, simply return null
            return null;

            Hole_Exists:
            // What if hole is in (x,0) ?            
            if (hole_pixel.Yi == 0)
            {
                var next_pixel = GetNextPixel(hole_pixel, img);
                while (next_pixel.Value == -1)
                {
                    hole_pixel = next_pixel;
                    next_pixel = GetNextPixel(hole_pixel, img);
                }
                _start = 4;
            }
            else
                _start = 0;

            _8i = _start;

            var Boundary = new List<Pixel>();

            var first = GetClockWisePixel(hole_pixel, img);                        
            Boundary.Add(first);

            var boundary_pixel = GetClockWisePixel(hole_pixel, img);

            // stop condition:
            //      A. reach the same first pixel we started from
            //      B. in cases of enclaves with 1 space gap, this might cause a premature stop
            //          we can make sure we are reaching it while completeing the full circle of the circle-wise turning
            //          i.e. that the turning index (_8i) == 0 (minus the extra step that is taken) 
            //          (also called Jacob's stopping criteria)
            while (!(boundary_pixel == first && _8i - 1 == _start))
            {
                if (boundary_pixel.Value != -1)
                {
                    if (!Boundary.Contains(boundary_pixel))
                        Boundary.Add(boundary_pixel);
                }
                else
                {
                    Backtrack();
                    hole_pixel = boundary_pixel;
                }
                boundary_pixel = GetClockWisePixel(hole_pixel, img);
            }

            return Boundary;
        }

        //      +---+---+---+
        //      | 1 | 2 | 3 |
        //      |nw | n |ne |
        //      +---+---+---+
        //      | 0 |   | 4 |
        //      | w |   | e |
        //      +---+---+---+
        //      | 7 | 6 | 5 |
        //      |sw | s |se |
        //      +---+---+---+

        private int[,] _8connected = new int[,] {
            {0, -1},    // 0 = w
            {-1, -1},   // 1 = nw
            {-1, 0},    // 2 = n
            {-1, 1},    // 3 = ne
            {0, 1},     // 4 = e
            {1, 1},     // 5 = se
            {1, 0},     // 6 = s
            {1, -1},    // 7 = sw    
        };

        private int _start; // how we first started the trip, 0 (west) for most cases, 4 (east) for holes touching west edge of image
        private int _8i;    // index to keep where are we in the clock-wise clock 
                            // 0 - w, 1 - nw, 2 - n, 3 - ne, 4 - e, 5 - se, 6 - s, 7 - sw

        private Pixel GetClockWisePixel(Pixel input, IImageMatrix img)
        {
            int new_x, new_y;
            do
            {
                var x_offset = _8connected[_8i, 0];
                var y_offset = _8connected[_8i, 1];

                _8i = (_8i + 1) % 8;

                new_x = input.Xi + x_offset;
                new_y = input.Yi + y_offset;
            }
            // if edge pixels, move to next clockwise
            while (new_x < 0 || new_x >= img.LenX || new_y < 0 || new_y >= img.LenY);

            return img.GetArrayElement(new_x, new_y);
        }

        private Pixel GetNextPixel(Pixel input, IImageMatrix img)
        {
            if (input.Yi + 1 < img.LenY)
            {
                return img.GetArrayElement(input.Xi, input.Yi + 1);
            }
            else if (input.Xi + 1 < img.LenX)
            {
                return img.GetArrayElement(input.Xi + 1, 0);
            }
            else
                return null;            
        }

        private void Backtrack()
        {
            // We want to go back to the last connected pixel we were in.
            // The return position might seem at first a bit redundant, as it returns us to a pixel already covered
            // it's crucial for the stop condition in certain cases... If we wouldn't mind missing enclaves
            // we could return one less to the next connected pixel not yet covered, and remove Jacob's stopping criteria...

            // There can be 2 cases where a new hole pixel was found in: 
            // diagonal - we will want to go counter clock 3 (+1 of the already advanced _8i) = -4 = +4
            //            _8i index will be +1, i.e. 2,4,6 or 0
            // straight - we will want to go counter clock 2 (+1 of the already advanced _8i) = -3 = +5
            //            _8i index will be +1, i.e. 1,3,5 or 7

            if (_8i % 2 == 1)
                _8i = (_8i + 5) % 8;
            else
                _8i = (_8i + 4) % 8;
        }
    }
}
