using HoleFilling.DataObjects;
using System.Collections.Generic;

namespace HoleFilling
{
    public class MooreTrace : ITraceAlgorithm
    {
        public List<Pixel> Trace(ImageMatrix img)
        {
            Pixel holePixel;
            for (int i = 0; i < img.LenX; i++)
                for (int j = 0; j < img.LenY; j++)
                {
                    holePixel = img.GetArrayElement(i, j);
                    if (holePixel.Value == -1)                                                               
                        // while goto statements are not highly approved of, as they create hard-to-read spaghetti code, 
                        // this (breaking out of nested loops) is one of the rare cases where they should be used, 
                        // as they are more coherent than setting up multiple flags, or using sub-methods that will return. 
                        goto Hole_Exists;                    
                }

            // if here - no hole was found, simply return null
            return null;

            Hole_Exists:
            // What if hole is in (x,0) ?            
            if (holePixel.Yi == 0)
            {
                var next_pixel = GetNextPixel(holePixel, img);
                while (next_pixel.Value == -1)
                {
                    holePixel = next_pixel;
                    next_pixel = GetNextPixel(holePixel, img);
                }
                _start = ClockWise.East;
            }
            else
                _start = ClockWise.West;

            _direction = _start;

            var Boundary = new List<Pixel>();

            var first = GetClockWisePixel(holePixel, img);                        
            Boundary.Add(first);

            var boundary_pixel = GetClockWisePixel(holePixel, img);

            // stop condition:
            //      A. reach the same first pixel we started from
            //      B. in cases of enclaves with 1 space gap, this might cause a premature stop
            //          we can make sure we are reaching it while completeing the full circle of the circle-wise turning
            //          i.e. that the turning index (_8i) == 0 (minus the extra step that is taken) 
            //          (also called Jacob's stopping criteria)
            while (!(boundary_pixel == first && _direction - 1 == _start))
            {
                if (boundary_pixel.Value != -1)
                {
                    if (!Boundary.Contains(boundary_pixel))
                        Boundary.Add(boundary_pixel);
                }
                else
                {
                    Backtrack();
                    holePixel = boundary_pixel;
                }
                boundary_pixel = GetClockWisePixel(holePixel, img);
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

        private int[,] _directionOffset = new int[,] {
            {0, -1},    // 0 = w
            {-1, -1},   // 1 = nw
            {-1, 0},    // 2 = n
            {-1, 1},    // 3 = ne
            {0, 1},     // 4 = e
            {1, 1},     // 5 = se
            {1, 0},     // 6 = s
            {1, -1},    // 7 = sw    
        };

        private ClockWise _start;       // how we first started the trip, 0 (west) for most cases, 4 (east) for holes touching west edge of image
        private ClockWise _direction;   // index to keep where are we in the clock-wise clock 
                                        // 0 - w, 1 - nw, 2 - n, 3 - ne, 4 - e, 5 - se, 6 - s, 7 - sw

        private Pixel GetClockWisePixel(Pixel input, ImageMatrix img)
        {
            int newX, newY;
            do
            {
                var x_offset = _directionOffset[(int)_direction, 0];
                var y_offset = _directionOffset[(int)_direction, 1];
                
                _direction = (ClockWise)((int)(_direction + 1) % 8);

                newX = input.Xi + x_offset;
                newY = input.Yi + y_offset;
            }
            // if edge pixels, move to next clockwise
            while (newX < 0 || newX >= img.LenX || newY < 0 || newY >= img.LenY);

            return img.GetArrayElement(newX, newY);
        }

        private Pixel GetNextPixel(Pixel input, ImageMatrix img)
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

        private const int BACKTRACK_STRAIGHT = 5;
        private const int BACKTRACK_DIAGONAL = 4;

        private void Backtrack()
        {
            // We want to go back to the last connected pixel we were in.
            // The return position might seem at first a bit redundant, as it returns us to a pixel already covered
            // it's crucial for the stop condition in certain cases... If we wouldn't mind missing enclaves
            // we could return one less to the next connected pixel not yet covered, and remove Jacob's stopping criteria...

            // There can be 2 cases where a new hole pixel was found in: 
            // straight - we will want to go counter clock 2 (+1 of the already advanced _8i) = -3 = +5
            // diagonal - we will want to go counter clock 3 (+1 of the already advanced _8i) = -4 = +4

            if ((int)_direction % 2 == 1)
                // straight - _direction index will be in 1,3,5 or 7
                _direction = (ClockWise)((int)(_direction + BACKTRACK_STRAIGHT) % 8);
            else
                // diagonal - _direction index will be in 2,4,6 or 0
                _direction = (ClockWise)((int)(_direction + BACKTRACK_DIAGONAL) % 8);
        }
    }
}
