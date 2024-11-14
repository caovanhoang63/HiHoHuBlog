using HiHoHuBlog.Modules.Search.Entity;
using HiHoHuBlog.Modules.Search.Repository.Implementation;
using HiHoHuBlog.Utils;
using Nest;
using Index = System.Index;

namespace HiHoHuBlog.Modules.Search.Repository.Interface;

public class EsSearchBlogRepository(EsClient client) : ISearchBlogRepository
{
    private ISearchBlogRepository _searchBlogRepositoryImplementation;

    public async Task<Result<Unit, Err>> AddBulkAsync(IEnumerable<Entity.BlogSearchDoc> blogs, DateTime? date)
    {
        var r = await client.Client.BulkAsync(b => b.Index<BlogSearchDoc>().CreateMany(blogs));

        if (!r.IsValid)
        {
            return Result<Unit, Err>.Err(UtilErrors.InternalServerError(r.OriginalException));
        }

        var migrationTimeDoc = new MigrationTimestamp()
        {
            Timestamp = date ?? DateTime.Now,
            Index = "blog_search"
        };

        var migrationResponse = await 
            client.Client
                .IndexAsync(migrationTimeDoc, i => i.Index<MigrationTimestamp>());

        if (!migrationResponse.IsValid)
        {
            return Result<Unit, Err>.Err(UtilErrors.InternalServerError(migrationResponse.OriginalException));
        }
    
        return Result<Unit, Err>.Ok(new Unit());
    }


    public async Task<Result<MigrationTimestamp?, Err>> GetLastMigrateTime()
    {
        var searchResponse = await client.Client.SearchAsync<MigrationTimestamp>(s => s
            .Index<MigrationTimestamp>()
            .Query(q => q
                .Term(t => t
                    .Field(f => f.Index)
                    .Value("blog")
                )
            )
            .Sort(sort => sort
                    .Descending("_id") 
            )
            .Size(1)
        );

        
        if (searchResponse.IsValid && searchResponse.Documents.Any())
        {
            var lastDocument = searchResponse.Documents.First();

            return Result<MigrationTimestamp?, Err>.Ok(lastDocument);
        }
    
        return Result<MigrationTimestamp?, Err>.Err(new Err("No documents found or an error occurred"));
    }
}