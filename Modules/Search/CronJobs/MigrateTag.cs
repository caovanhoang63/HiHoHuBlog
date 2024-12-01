using HiHoHuBlog.Modules.Search.Service.Interface;
using Quartz;

namespace HiHoHuBlog.Modules.Search.CronJobs;

public class MigrateTag(IMigrationSearchDataService migrationService) : IJob
{

    private readonly IMigrationSearchDataService _migrationService = migrationService;

    public async Task Execute(IJobExecutionContext context)
    {
        Console.WriteLine("Migrating Tag...");
        var r =  await _migrationService.MigrateTagDataAsync();

        if (r.IsOk)
        {
            Console.WriteLine("Migrated Tag.");
        }
        else
        {
            Console.WriteLine($"Failed to migrate Tag with Error: {r.Error}");
        }
    }
    
}