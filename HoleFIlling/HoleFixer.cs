using HoleFilling.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HoleFilling
{
    public class HoleFixer : IHoleFixer
    {
        private ImageMatrix _image;

        public HoleFixer(ImageMatrix image)
        {
            _image = image;
        }

        /// <summary>
        /// Fixes hole. Default implementation of interface - using default weight function
        /// </summary>
        /// <param name="hole"></param>
        public void FixHole(Hole hole)
        {
            FillHoleWithWeightFunction(hole);
        }

        /// <summary>
        /// Fixes hole. Default implementation of interface - using supplied weight function
        /// </summary>
        /// <param name="hole"></param>
        public void FixHole(Hole hole, IWeightFunction weightFunction)
        {
            FillHoleWithWeightFunction(hole, weightFunction);
        }

        /// <summary>
        /// Fills holes using weight function
        /// </summary>
        public void FillHoleWithWeightFunction(Hole hole)
        {
            var weightFunction = new DefaultWeightFunction(new Dictionary<string, object>
            {
                ["z"] = 5,
                ["e"] = 0.0001
            });

            FillHoleWithWeightFunction(hole, weightFunction);
        }

        /// <summary>
        /// Fills holes using weight function
        /// </summary>
        /// <param name="weightFunction"></param>
        public void FillHoleWithWeightFunction(Hole hole, IWeightFunction weightFunction)
        {
            Fix(hole, weightFunction);

            _image.IsHoled = false;
        }

        private void Fix(Hole hole, IWeightFunction weightFunction)
        {
            foreach (var pix in hole.HolePixels)
                FillPixelWithWeightFunction(hole, pix, weightFunction);
        }

        private void FillPixelWithWeightFunction(Hole hole, Pixel pix, IWeightFunction weightFunction)
        {
            if (pix.Value != -1)
                return;

            float numerator = 0;
            float denominator = 0;

            foreach (var yi in hole.Boundary)
            {
                var w = weightFunction.GetWeight(yi, pix);
                numerator += (w * yi.Value);
                denominator += w;
            }

            pix.Value = numerator / denominator;
        }

        /// <summary>
        /// Fills hole using average approximation
        /// </summary>
        public void FillHoleApproximateAverage(Hole hole)
        {
            float sum = 0;
            foreach (var b in hole.Boundary)
                sum += b.Value;
            var avg = sum / hole.Boundary.Count;

            foreach (var pix in hole.HolePixels)
                pix.Value = avg;

            hole = null;
            _image.IsHoled = false;
        }


        /// <summary>
        /// Gradient approximation to fill hole - uses the 4 edges of a covering rectangle to fill each pixel based on its distance from them.
        /// Throws error if hole touches the edges of the image.
        /// </summary>
        public void FillHoleApproximateGradient(Hole hole)
        {
            if (hole.CoveringRectangle.TopRight.Value == -1
                || hole.CoveringRectangle.BottomLeft.Value == -1
                || hole.CoveringRectangle.TopLeft.Value == -1
                || hole.CoveringRectangle.BottomRight.Value == -1)
                throw new Exception("Better-Approximation does not work if the hole touches the edges of the image.");

            var horizontalDistance = hole.CoveringRectangle.BottomRight.Yi - hole.CoveringRectangle.TopLeft.Yi;
            var verticalDistance = hole.CoveringRectangle.BottomRight.Xi - hole.CoveringRectangle.TopLeft.Xi;

            foreach (var pix in hole.HolePixels)
            {
                var distanceToStart = DistanceMeasures.ChebyshevDistance(pix, hole.CoveringRectangle.TopLeft);
                var distanceToEnd = DistanceMeasures.ChebyshevDistance(pix, hole.CoveringRectangle.BottomRight);

                float pixValue;
                if (distanceToStart <= distanceToEnd)
                {
                    var valueY = (hole.CoveringRectangle.TopRight.Value - hole.CoveringRectangle.TopLeft.Value) *
                        (pix.Yi - hole.CoveringRectangle.TopLeft.Yi) / horizontalDistance;
                    var valueX = (hole.CoveringRectangle.BottomLeft.Value - hole.CoveringRectangle.TopLeft.Value) *
                        (pix.Xi - hole.CoveringRectangle.TopLeft.Xi) / verticalDistance;
                    pixValue = hole.CoveringRectangle.TopLeft.Value + valueX + valueY;
                }
                else
                {
                    var valueY = (hole.CoveringRectangle.BottomLeft.Value - hole.CoveringRectangle.BottomRight.Value) *
                        (hole.CoveringRectangle.BottomRight.Yi - pix.Yi) / horizontalDistance;
                    var valueX = (hole.CoveringRectangle.TopRight.Value - hole.CoveringRectangle.BottomRight.Value) *
                        (hole.CoveringRectangle.BottomRight.Xi - pix.Xi) / verticalDistance;
                    pixValue = hole.CoveringRectangle.BottomRight.Value + valueX + valueY;
                }

                if (pixValue > 1)
                    pixValue = 1;
                else if (pixValue < 0)
                    pixValue = 0;

                pix.Value = pixValue;
            }

            _image.IsHoled = false;
        }

        /// <summary>
        /// Fills hole by approximation of its 8-connected non-hole pixels; Does so in a spiral fashion.
        /// Throws error if hole touches the edges of the image.
        /// </summary>
        public void FillHoleApproximateConnected(Hole hole)
        {
            if (hole.CoveringRectangle.TopRight.Value == -1
                || hole.CoveringRectangle.BottomLeft.Value == -1
                || hole.CoveringRectangle.TopLeft.Value == -1
                || hole.CoveringRectangle.BottomRight.Value == -1)
                throw new Exception("Better-Approximation does not work if the hole touches the edges of the image.");

            var spiralTraverser = new SpiralTraverser(hole.CoveringRectangle, _image);
            foreach (var pix in spiralTraverser)
            {
                if (pix.Value == -1)
                {
                    var array = new Pixel[]
                    {
                    _image.GetDistantElement(pix, 0, -1),
                    _image.GetDistantElement(pix, -1, -1),
                    _image.GetDistantElement(pix, -1, 0),
                    _image.GetDistantElement(pix, -1, 1),
                    _image.GetDistantElement(pix, 0, 1),
                    _image.GetDistantElement(pix, 1, 1),
                    _image.GetDistantElement(pix, 1, 0),
                    _image.GetDistantElement(pix, 1, -1)
                    }
                    .Where(x => x != null && x.Value != -1);

                    var sum = array.Sum(x => x.Value);
                    var count = array.Count();

                    pix.Value = sum / count;
                }
            }

            _image.IsHoled = false;
        }
    }
}
