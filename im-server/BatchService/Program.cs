using BatchService.Jobs;
using IMLibrary.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BatchService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    // Add the required Quartz.NET services
                    services.AddQuartz(q =>
                    {
                        q.UseMicrosoftDependencyInjectionJobFactory();

                        q.AddJobAndTrigger<WeightedScoreJob>(hostContext.Configuration);
                    });

                    // Add the Quartz.NET hosted service
                    services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

                    // other config
                    services.AddDbContext<IM_API_Context>(options =>
                            options.UseMySQL(hostContext.Configuration.GetConnectionString("IM_API_Context")));

                    IMLibrary.Helpers.DependencyConfig.AddDependencies(services);
                });
    }
}
