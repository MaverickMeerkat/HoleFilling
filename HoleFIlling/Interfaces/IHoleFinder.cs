using HoleFilling.DataObjects;

namespace HoleFilling
{
    public interface IHoleFinder
    {
        Hole Hole { get; }
        Hole FindHole(ITraceAlgorithm trace);
    }
}
