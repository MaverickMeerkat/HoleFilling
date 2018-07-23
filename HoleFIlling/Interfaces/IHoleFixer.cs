using HoleFilling.DataObjects;

namespace HoleFilling
{
    public interface IHoleFixer
    {
        void FixHole(Hole hole);
        void FixHole(Hole hole, IWeightFunction weightFunction);        
    }
}
