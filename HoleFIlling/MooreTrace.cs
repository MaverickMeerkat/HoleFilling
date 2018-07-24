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
            ClockWise _start; // how we first start the trip
            _direction = ClockWise.West; // initial position
            // What if hole is in (x,0) ? We'll go around it clockwise until we find a non-hole pixel
            if (holePixel.Y == 0)
            {
                var nextPixel = GetClockWisePixel(holePixel, img);
                while (nextPixel.Value == -1)
                {
                    Backtrack();
                    holePixel = nextPixel;
                    nextPixel = GetClockWisePixel(holePixel, img);
                }
                _direction = (ClockWise)((int)(_direction + 7) % 8);
            }

            _start = _direction;

            var Boundary = new List<Pixel>();

            var firstPixel = GetClockWisePixel(holePixel, img);                        
            Boundary.Add(firstPixel);

            var boundaryPixel = GetClockWisePixel(holePixel, img);

            // stop condition:
            //      A. reach the same first pixel we started from
            //      B. in cases of enclaves with 1 space gap, this might cause a premature stop
            //          we can make sure we are reaching it while completeing the full circle of the circle-wise turning            
            //          (also called Jacob's stopping criteria)
            while (!(boundaryPixel == firstPixel && _direction - 1 == _start))
            {
                if (boundaryPixel.Value != -1)
                {
                    if (!Boundary.Contains(boundaryPixel))
                        Boundary.Add(boundaryPixel);
                }
                else
                {
                    Backtrack();
                    holePixel = boundaryPixel;
                }
                boundaryPixel = GetClockWisePixel(holePixel, img);
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

                newX = input.X + x_offset;
                newY = input.Y + y_offset;
            }
            // if edge pixels, move to next clockwise
            while (newX < 0 || newX >= img.LenX || newY < 0 || newY >= img.LenY);

            return img.GetArrayElement(newX, newY);
        }

        private const int BACKTRACK_STRAIGHT = 5;
        private const int BACKTRACK_DIAGONAL = 4;

        private void Backtrack()
        {
            // We want to go back to the last connected pixel we were in.
            // There can be 2 cases where a new hole pixel was found in: 
            // straight - we will want to go counter clock 2 (+1 of the already advanced _direction) = -3 = +5
            // diagonal - we will want to go counter clock 3 (+1 of the already advanced _direction) = -4 = +4

            if ((int)_direction % 2 == 1)
                // straight - _direction index will be in 1,3,5 or 7
                _direction = (ClockWise)((int)(_direction + BACKTRACK_STRAIGHT) % 8);
            else
                // diagonal - _direction index will be in 2,4,6 or 0
                _direction = (ClockWise)((int)(_direction + BACKTRACK_DIAGONAL) % 8);
        }
    }
}
