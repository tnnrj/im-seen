using IMWebAPI.Data;
using IMWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMWebAPI.Helpers
{
    public class WeightedScoreCalculator
    {
        private readonly IM_API_Context _context;
        private readonly double numDays;

        public WeightedScoreCalculator(IM_API_Context context)
        {
            _context = context;
            numDays = 7;
        }

        public double CalculateObservationFrequencyIncrease (Student student, double prevFrequency, out double newFrequency)
        {
            var observs = _context.Observations
                .Where(o => o.StudentID == student.StudentID)
                .Where(o => o.ObservationDate >= DateTime.Now.AddDays(-numDays)).ToList();

            newFrequency = observs.Count / numDays;

            return newFrequency / prevFrequency;
        }
    }
}
