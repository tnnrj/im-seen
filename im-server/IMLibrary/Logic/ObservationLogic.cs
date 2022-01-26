using IMLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMLibrary.Logic
{
    public interface IObservationLogic
    {
        void CalculateWeightedScore(Observation obs);
    }

    public class ObservationLogic : IObservationLogic
    {
        public ObservationLogic()
        {

        }

        public void CalculateWeightedScore(Observation obs)
        {

        }
    }
}
