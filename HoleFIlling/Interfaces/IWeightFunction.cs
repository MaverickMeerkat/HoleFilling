using HoleFilling.DataObjects;
using System.Collections.Generic;

namespace HoleFilling
{
    public interface IWeightFunction
    {
        float GetWeight(Pixel x, Pixel y);
        void SetAdditionalParameters(Dictionary<string, object> additionalParams);
    }

    public abstract class WeightInitialization
    {
        private readonly Dictionary<string, object> additionalParameters;

        public WeightInitialization(Dictionary<string, object> additionalParameters)
        {
            this.additionalParameters = additionalParameters;
        }
    }
}
