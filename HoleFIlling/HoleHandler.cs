using System;
using System.Collections.Generic;

namespace HoleFilling
{
    public class HoleHandler : IHoleHandler
    {
        private ImageMatrix _image;

        public List<Pixel> Boundary { get; set; }
        public Hole Hole { get; set; }
        public IWeightFunction WeightFunction { get; set; }

        public HoleHandler(ImageMatrix image) : this(image, null)
        {
            WeightFunction = new DefaultWeightFunction(new Dictionary<string, object>
            {
                ["z"] = 5,
                ["e"] = 0.0001
            });
        }

        public HoleHandler(ImageMatrix image, IWeightFunction weight)
        {
            _image = image;
            WeightFunction = weight;
        }

        /// <summary>
        /// Finds a boundary of a hole
        /// </summary>
        /// <param name="trace"></param>
        /// <returns></returns>
        public List<Pixel> FindBoundary(ITraceAlgorithm trace)
        {
            Boundary = trace.Trace(_image);
            return Boundary;
        }

        /// <summary>
        /// Creates a hole in an image matrix
        /// </summary>
        /// <param name="x_start"></param>
        /// <param name="x_stop"></param>
        /// <param name="y_start"></param>
        /// <param name="y_stop"></param>
        public void CreateHole(int x_start, int x_stop, int y_start, int y_stop)
        {
            for (int i = x_start; i < x_stop; i++)
                for (int j = y_start; j < y_stop; j++)
                {
                    var pix = _image.GetArrayElement(i, j);
                    pix.Value = -1;
                }

            _image.IsHoled = true;
        }

        /// <summary>
        /// Finds the hole circumferenced by the boundary
        /// </summary>
        /// <returns></returns>
        public Hole FindHole()
        {
            if (Boundary == null)
                return null;

            Hole = new Hole();

            // find covering rectangle over hole
            Hole.CoveringRectangle = FindCoveringRectangle();

            // go over that rectangle and push hole pixels to list
            for (int i = Hole.CoveringRectangle.TopLeft.Xi; i <= Hole.CoveringRectangle.BottomRight.Xi; i++)
                for (int j = Hole.CoveringRectangle.TopLeft.Yi; j <= Hole.CoveringRectangle.BottomRight.Yi; j++)
                {
                    var pix = _image.GetArrayElement(i, j);
                    if (pix.Value == -1)
                        Hole.HolePixels.Add(pix);
                }

            return Hole;
        }

        private Rectangle FindCoveringRectangle()
        {
            int[] min = new int[2] { _image.LenX - 1, _image.LenY - 1 };
            int[] max = new int[2] { 0, 0 };

            foreach (var pix in Boundary)
            {
                if (pix.Xi < min[0])
                    min[0] = pix.Xi;
                if (pix.Xi > max[0])
                    max[0] = pix.Xi;
                if (pix.Yi < min[1])
                    min[1] = pix.Yi;
                if (pix.Yi > max[1])
                    max[1] = pix.Yi;
            }

            var coveringRectangle = new Rectangle(
                _image.GetArrayElement(min[0], min[1]),
                _image.GetArrayElement(max[0], max[1]),
                _image.GetArrayElement(min[0], max[1]),
                _image.GetArrayElement(max[0], min[1])
                );
            return coveringRectangle;
        }

        /// <summary>
        /// Fill rectangular shaped holes based on supplied rectangle positions; Uses weight function to fill
        /// </summary>
        /// <param name="x_start"></param>
        /// <param name="x_end"></param>
        /// <param name="y_start"></param>
        /// <param name="y_end"></param>
        public void FillHole(int x_start, int x_end, int y_start, int y_end)
        {
            for (int i = x_start; i < x_end; i++)
                for (int j = y_start; j < y_end; j++)
                    FillPixelWithWeightFunction(i, j);

            Boundary = null;
            Hole = null;
            _image.IsHoled = false;
        }

        /// <summary>
        /// Will calculate hole positions by itself, and fill it; Uses weight function
        /// </summary>
        public void FillHole()
        {
            if (Hole == null)
                FindHole();
            foreach (var pix in Hole.HolePixels)
                FillPixelWithWeightFunction(pix);

            Boundary = null;
            Hole = null;
            _image.IsHoled = false;
        }

        private void FillPixelWithWeightFunction(int locX, int locY)
        {
            FillPixelWithWeightFunction(_image.GetArrayElement(locX, locY));
        }

        private void FillPixelWithWeightFunction(Pixel hole)
        {
            if (hole.Value != -1)
                return;

            float numerator = 0;
            float denominator = 0;

            foreach (var yi in Boundary)
            {
                var w = WeightFunction.GetWeight(yi, hole);
                numerator += (w * yi.Value);
                denominator += w;
            }

            hole.Value = numerator / denominator;
        }

        /// <summary>
        /// Will calculate hole positions by itself, and fill it; Uses average approximation
        /// </summary>
        public void FillHoleApproximate()
        {
            float sum = 0;
            foreach (var b in Boundary)
                sum += b.Value;
            var avg = sum / Boundary.Count;

            if (Hole == null)
                FindHole();

            foreach (var pix in Hole.HolePixels)
                pix.Value = avg;

            Boundary = null;
            Hole = null;
            _image.IsHoled = false;
        }


        /// <summary>
        /// Better (gradient) approximation to fill hole - uses the 4 edges of a covering rectangle to fill each pixel based on its distance from them.
        /// Throws error if hole touches the edges of the image.
        /// </summary>
        public void FillHoleBetterApproximate()
        {
            if (Hole == null)
                FindHole();

            if (Hole.CoveringRectangle.TopRight.Value == -1
                || Hole.CoveringRectangle.BottomLeft.Value == -1
                || Hole.CoveringRectangle.TopLeft.Value == -1
                || Hole.CoveringRectangle.BottomRight.Value == -1)
                throw new Exception("Better-Approximation does not work if the hole touches the edges of the image.");

            var horizontalDistane = Hole.CoveringRectangle.BottomRight.Yi - Hole.CoveringRectangle.TopLeft.Yi;
            var verticalDistance = Hole.CoveringRectangle.BottomRight.Xi - Hole.CoveringRectangle.TopLeft.Xi;

            foreach (var pix in Hole.HolePixels)
            {
                var distanceToStart = DistanceMeasures.ChebyshevDistance(pix, Hole.CoveringRectangle.TopLeft);
                var distanceToEnd = DistanceMeasures.ChebyshevDistance(pix, Hole.CoveringRectangle.BottomRight);

                var pixValue = pix.Value;
                if (distanceToStart <= distanceToEnd)
                {
                    var valueY = (Hole.CoveringRectangle.TopRight.Value - Hole.CoveringRectangle.TopLeft.Value) * 
                        (pix.Yi - Hole.CoveringRectangle.TopLeft.Yi) / horizontalDistane;
                    var valueX = (Hole.CoveringRectangle.BottomLeft.Value - Hole.CoveringRectangle.TopLeft.Value) * 
                        (pix.Xi - Hole.CoveringRectangle.TopLeft.Xi) / verticalDistance;
                    pixValue = Hole.CoveringRectangle.TopLeft.Value + valueX + valueY;                    
                }
                else
                {
                    var valueY = (Hole.CoveringRectangle.BottomLeft.Value - Hole.CoveringRectangle.BottomRight.Value) * 
                        (Hole.CoveringRectangle.BottomRight.Yi - pix.Yi) / horizontalDistane;
                    var valueX = (Hole.CoveringRectangle.TopRight.Value - Hole.CoveringRectangle.BottomRight.Value) * 
                        (Hole.CoveringRectangle.BottomRight.Xi - pix.Xi) / verticalDistance;
                    pixValue = Hole.CoveringRectangle.BottomRight.Value + valueX + valueY;
                }

                if (pixValue > 1)
                    pixValue = 1;
                else if (pixValue < 0)
                    pixValue = 0;

                pix.Value = pixValue;
            }

            Boundary = null;
            Hole = null;
            _image.IsHoled = false;

        }
    }
}
