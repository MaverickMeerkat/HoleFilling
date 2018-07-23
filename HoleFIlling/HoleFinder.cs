using HoleFilling.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoleFilling
{
    public class HoleFinder : IHoleFinder
    {
        public Hole Hole { get; private set; }

        private readonly ImageMatrix _image;

        public HoleFinder(ImageMatrix image)
        {
            _image = image;
        }

        /// <summary>
        /// Default implementation is with Moore-Trace Algorithm
        /// </summary>
        /// <returns></returns>
        public Hole FindHole()
        {
            return FindHole(new MooreTrace());
        }

        /// <summary>
        /// Finds the hole in the image matrix, based on the supplied boundary trace algorithm
        /// </summary>
        /// <param name="trace">A trace algorithm to be used to find the boundary of the hole</param>
        /// <returns></returns>
        public Hole FindHole(ITraceAlgorithm trace)
        {
            var boundary = FindBoundary(trace);
            var rectangle = FindCoveringRectangle(boundary);
            var hole = FindHole(rectangle);

            if (boundary != null)
                Hole = new Hole(boundary, hole, rectangle);
            else
                Hole = null;

            return Hole;
        }

        private List<Pixel> FindBoundary(ITraceAlgorithm trace)
        {
            return trace.Trace(_image);
        }

        private Rectangle FindCoveringRectangle(List<Pixel> boundary)
        {
            if (boundary == null)
                return null;

            int[] min = new int[2] { _image.LenX - 1, _image.LenY - 1 };
            int[] max = new int[2] { 0, 0 };

            foreach (var pix in boundary)
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

        private List<Pixel> FindHole(Rectangle coverRectangle)
        {
            if (coverRectangle == null)
                return null;

            var list = new List<Pixel>();
            // go over that rectangle and push hole pixels to list
            for (int i = coverRectangle.TopLeft.Xi; i <= coverRectangle.BottomRight.Xi; i++)
                for (int j = coverRectangle.TopLeft.Yi; j <= coverRectangle.BottomRight.Yi; j++)
                {
                    var pix = _image.GetArrayElement(i, j);
                    if (pix.Value == -1)
                        list.Add(pix);
                }

            return list;
        }

    }
}
