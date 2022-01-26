/**
 * This file defines a helper class for calculating weighted scores of Observations. These calculations should be altered as needed by administrators.
 * Written by Steven Carpadakis, U of U School of Computing, Senior Capstone 2021
 **/

using IMLibrary.Data;
using IMLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMLibrary.Helpers
{
    public class WeightedScoreCalculator
    {
        private readonly IM_API_Context _context;
        private readonly int numDays;
        private readonly double decayRate;

        public WeightedScoreCalculator(IM_API_Context context)
        {
            _context = context;
            numDays = 2;
            decayRate = 0.5;
        }

        public double CalculateObservationWeightedScore (Observation observ)
        {
            double score = observ.Severity;
            TimeSpan threshold = new TimeSpan(numDays, 0, 0, 0);

            TimeSpan timeSinceCreated = DateTime.UtcNow.Subtract(observ.ObservationDate);

            if (timeSinceCreated.Days > numDays)
                score = score * decayRate * (timeSinceCreated.Days / numDays);

            return score;
        }

    }
}
