using IMLibrary.Logic;
using IMLibrary.Models;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BatchService.Jobs
{
    /// <summary>
    /// Batch job to recalculate weighted scores daily
    /// </summary>
    [DisallowConcurrentExecution]
    class WeightedScoreJob : IJob
    {
        private readonly ILogger<WeightedScoreJob> _logger;
        private readonly IObservationLogic _observationLogic;

        public WeightedScoreJob(ILogger<WeightedScoreJob> logger, IObservationLogic observationLogic)
        {
            _logger = logger;
            _observationLogic = observationLogic;
        }

        public Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("Starting weighted score recalculation...");
            _observationLogic.CalculateWeightedScore(new Observation());
            return Task.CompletedTask;
        }
    }
}
