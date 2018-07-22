using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoleFilling
{
    interface IHoleHandler
    {
        List<Pixel> Boundary { get; set; }
        Hole Hole { get; set; }
        IWeightFunction WeightFunction { get; set; }

        List<Pixel> FindBoundary(ITraceAlgorithm trace);
        void CreateHole(int x_start, int x_stop, int y_start, int y_stop);
        Hole FindHole();
        void FillHole();
    }
}
