using IMLibrary.Data;
using IMLibrary.Helpers;
using IMLibrary.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace IMLibrary.Logic
{
    public interface IObservationLogic
    {
        int RecalcAllScores();
        double CalculateWeightedScore(Observation obs);
    }

    public class ObservationLogic : IObservationLogic
    {
        private readonly IM_API_Context _context;
        private readonly IRuntimeConfig _runtimeConfig;

        public ObservationLogic(IM_API_Context context, IRuntimeConfig runtimeConfig)
        {
            _context = context;
            _runtimeConfig = runtimeConfig;
        }

        public int RecalcAllScores()
        {
            var cfg = _runtimeConfig.GetConfigDictionary();
            var relevantDays = int.Parse(cfg["DaysToDecay"]) + int.Parse(cfg["RelevantDaysWindow"]);
            var startDate = DateTime.Today.AddDays(-relevantDays);
            var observations = _context.Observations.Where(o => o.ObservationDate >= startDate).ToList();

            foreach (var toRecalc in observations.Where(o => )
        }

        public double CalculateWeightedScore(Observation obs, Dictionary<string, string> cfg = null)
        {
            if (cfg == null)
            {
                cfg = _runtimeConfig.GetConfigDictionary();
            }
            var score = (double)obs.Severity;

            var daysToDecay = int.Parse(cfg["DaysToDecay"]);
            var daysPassed = DateTime.Now.Subtract(obs.ObservationDate).Days;
            score *= (daysToDecay - daysPassed) / daysToDecay;
            if (score <= 0) return 0.0;

            var relevantDaysWindow = int.Parse(cfg["RelevantDaysWindow"]);

            if (cfg.ContainsKey(obs.Status + "StatusModifier"))
            {
                score += double.Parse(cfg[obs.Status + "StatusModifier"]);
            }

            return score < 0 ? 0 : score;
        }
    }
}
