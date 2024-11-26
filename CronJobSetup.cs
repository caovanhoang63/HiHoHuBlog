using HiHoHuBlog.Modules.Search.CronJobs;
using Quartz;

namespace HiHoHuBlog;

public class CronJobSetup
{
    public static void SetUp(IServiceCollection service)
    {
        service.AddQuartz(q =>
        {
            var jobKey = new JobKey("MigrateBlog");
            q.AddJob<MigrateBlog>(opts => opts.WithIdentity(jobKey));

            q.AddTrigger(opts => opts
                .ForJob(jobKey) 
                .WithIdentity("ExampleJob-trigger")
                .WithCronSchedule("0 0 * * * ?")); 
        });
        service.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
    }
    
}