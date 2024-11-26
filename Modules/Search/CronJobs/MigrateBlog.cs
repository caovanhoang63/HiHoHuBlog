using HiHoHuBlog.Modules.Search.Service.Interface;
using Quartz;

namespace HiHoHuBlog.Modules.Search.CronJobs;

public class MigrateBlog(IMigrationSearchDataService migrationService) : IJob
{
    private readonly IMigrationSearchDataService _migrationService = migrationService;

    public async Task Execute(IJobExecutionContext context)
    {
        Console.WriteLine("Migrating blog...");
         var r =  await _migrationService.MigrateBlogDataAsync();

         if (r.IsOk)
         {
             Console.WriteLine("Migrated blog.");
         }
         else
         {
             Console.WriteLine($"Failed to migrate blog with Error: {r.Error}");
         }
    }
}