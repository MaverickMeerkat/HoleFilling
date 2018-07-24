using HoleFilling.DataObjects;
using System.Collections.Generic;

namespace HoleFilling
{
    public interface ITraceAlgorithm
    {
        List<Pixel> Trace(ImageMatrix img);
    }
}
