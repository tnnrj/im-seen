using IMLibrary.Logic;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace IMLibrary.Helpers
{
    public static class DependencyConfig
    {
        public static void AddDependencies(IServiceCollection services)
        {
            // update services for dependency injection
            services.AddScoped<IEmailer, Emailer>();
            services.AddScoped<IQueryRunner, QueryRunner>();
            services.AddScoped<IRuntimeConfig, RuntimeConfig>();

            services.AddScoped<IObservationLogic, ObservationLogic>();
        }
    }
}
