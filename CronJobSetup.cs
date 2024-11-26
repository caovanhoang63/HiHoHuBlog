using HiHoHuBlog.Modules.Search.CronJobs;
using Quartz;

namespace HiHoHuBlog;

public class CronJobSetup
{
    public static void SetUp(IServiceCollection service)
    {
        service.AddQuartz(q =>
        {
            var migrateBlogKey = new JobKey("MigrateBlog");
            q.AddJob<MigrateBlog>(opts => opts.WithIdentity(migrateBlogKey));
            q.AddTrigger(opts => opts
                .ForJob(migrateBlogKey) 
                .WithIdentity("MigrateBlogJob-trigger")
                .WithCronSchedule("0 0 * * * ?")); 
            
            var migrateTagKey = new JobKey("MigrateTag");
            q.AddJob<MigrateTag>(opts => opts.WithIdentity(migrateTagKey));
            q.AddTrigger(opts => opts
                .ForJob(migrateTagKey) 
                .WithIdentity("MigrateTagJob-trigger")
                .WithCronSchedule("0 0 * * * ?")); 
            
            var migrateUserKey = new JobKey("MigrateUser");
            q.AddJob<MigrateUser>(opts => opts.WithIdentity(migrateUserKey));
            q.AddTrigger(opts => opts
                .ForJob(migrateUserKey) 
                .WithIdentity("MigrateUserJob-trigger")
                .WithCronSchedule("0 0 * * * ?")); 
            
        });
        
        service.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
    }
    
}