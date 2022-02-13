using IMLibrary.Data;
using IMLibrary.Helpers;
using IMLibrary.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace IMLibrary.Logic
{
    public interface IObservationLogic
    {
        /// <summary>
        /// Recalculates scores for all recent observations
        /// </summary>
        void RecalcAllScores(out int success, out int fail);

        /// <summary>
        /// Calculates the weighted score for an observation
        /// </summary>
        double CalculateWeightedScore(Observation obs, IList<Observation> neighbors = null, IDictionary<string, string> cfg = null);

        /// <summary>
        /// Determines if a change to an observation requires recalculating its own score
        /// </summary>
        bool NeedsScoreRecalc(Observation oldObservation, Observation newObservation);

        /// <summary>
        /// Returns a student matching an incoming observation, or null if no match is found
        /// </summary>
        Student MatchingStudent(Observation obs);
    }

    public class ObservationLogic : IObservationLogic
    {
        private readonly IM_API_Context _context;
        private readonly IRuntimeConfig _runtimeConfig;
        private readonly ILogger<IObservationLogic> _logger;

        public ObservationLogic(IM_API_Context context, IRuntimeConfig runtimeConfig, ILogger<IObservationLogic> logger)
        {
            _context = context;
            _runtimeConfig = runtimeConfig;
            _logger = logger;
        }

        public void RecalcAllScores(out int success, out int failure)
        {
            var cfg = _runtimeConfig.GetConfigDictionary();
            var daysToDecay = int.Parse(cfg["DaysToDecay"]);
            var relevantDaysWindow = int.Parse(cfg["RelevantDaysWindow"]);
            success = 0;
            failure = 0;

            var startDate = DateTime.Today.AddDays(-daysToDecay);
            var relevantDate = startDate.AddDays(-relevantDaysWindow);
            var observations = _context.Observations.Where(o => o.ObservationDate >= relevantDate).ToList();

            if (!observations.Any())
            {
                return;
            }

            // iterate over observations needing recalculation
            foreach (var obs in observations.Where(o => o.ObservationDate >= startDate))
            {
                try
                {
                    var start = obs.ObservationDate.AddDays(-relevantDaysWindow);
                    var end = obs.ObservationDate.AddDays(relevantDaysWindow);
                    var neighbors = observations.Where(o => o.StudentID == obs.StudentID && o.ObservationDate >= start && o.ObservationDate <= end && o.ObservationID != obs.ObservationID).ToList();
                    obs.WeightedScore = CalculateWeightedScore(obs, neighbors, cfg);
                    _context.SaveChanges();
                    success++;
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Error during observation recalculation");
                    failure++;
                }
            }
        }

        public double CalculateWeightedScore(Observation obs, IList<Observation> neighbors = null, IDictionary<string, string> cfg = null)
        {
            if (cfg == null)
            {
                cfg = _runtimeConfig.GetConfigDictionary();
            }
            var score = (double)obs.Severity;

            // ------ RECENCY MODIFIER ------
            var daysToDecay = double.Parse(cfg["DaysToDecay"]);
            var daysPassed = (double)DateTime.Now.Subtract(obs.ObservationDate).Days;
            // linear decay by days passed
            score *= (daysToDecay - daysPassed) / daysToDecay;

            // ------ FREQUENCY MODIFIER ------
            var relevantDaysWindow = double.Parse(cfg["RelevantDaysWindow"]);
            if (neighbors == null)
            {
                var start = obs.ObservationDate.AddDays(-relevantDaysWindow);
                var end = obs.ObservationDate.AddDays(relevantDaysWindow);
                neighbors = _context.Observations.Where(o => o.StudentID == obs.StudentID && o.ObservationDate >= start && o.ObservationDate <= end && o.ObservationID != obs.ObservationID).ToList();
            }
            // moderate exponential decay on number of neighbors
            var decayConstant = double.Parse(cfg["FrequencyDecayConstant"]);
            score *= Math.Exp(-decayConstant * neighbors.Count());
            

            // ------ STATUS MODIFIER ------
            var statusKey = obs.Status + "StatusModifier";
            if (cfg.ContainsKey(statusKey))
            {
                if (cfg[statusKey] == "ZERO")
                {
                    score = 0;
                }
                else
                {
                    score += double.Parse(cfg[statusKey]);
                }
            }

            // never score an observation less than zero
            return score < 0 ? 0 : score;
        }

        public bool NeedsScoreRecalc(Observation oldObservation, Observation newObservation)
        {
            // only recalculate if relevant fields have changed and the score has not already decayed to zero
            if ((newObservation.Severity != oldObservation?.Severity || newObservation.Status != oldObservation?.Status)
                && newObservation.ObservationDate >= DateTime.Now.AddDays(-int.Parse(_runtimeConfig.GetConfigDictionary()["DaysToDecay"])))
            {
                return true;
            }

            return false;
        }

        public Student MatchingStudent(Observation obs)
        {
            // current logic is exact match by name
            return _context.Students
                .Where(s => (s.FirstName == obs.StudentFirstName) && (s.LastName == obs.StudentLastName))
                .FirstOrDefault();
        }
    }
}
