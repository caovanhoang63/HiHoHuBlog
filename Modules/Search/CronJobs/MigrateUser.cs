using HiHoHuBlog.Modules.Search.Service.Interface;
using Quartz;

namespace HiHoHuBlog.Modules.Search.CronJobs;

public class MigrateUser(IMigrationSearchDataService migrationService) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        Console.WriteLine("Migrating User...");
        var r =  await migrationService.MigrateUserDataAsync();

        if (r.IsOk)
        {
            Console.WriteLine("Migrated User.");
        }
        else
        {
            Console.WriteLine($"Failed to migrate User with Error: {r.Error}");
        }
    }
}