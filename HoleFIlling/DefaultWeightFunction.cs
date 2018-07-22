using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoleFilling
{
    public class DefaultWeightFunction : WeightInitialization, IWeightFunction
    {
        private float _z;
        private float _e;

        public DefaultWeightFunction(Dictionary<string, object> additionalParams) : base(additionalParams)
        {
            _z = Convert.ToSingle(additionalParams["z"]);
            _e = Convert.ToSingle(additionalParams["e"]);
        }

        public float GetWeight(Pixel x, Pixel y)
        {
            return (1 / (_e + (float)Math.Pow(DistanceMeasures.ChebyshevDistance(x, y), _z)));

        }

        public void SetAdditionalParameters(Dictionary<string, object> additionalParams)
        {
            _z = Convert.ToSingle(additionalParams["z"]);
            _e = Convert.ToSingle(additionalParams["e"]);
        }
    }
}
