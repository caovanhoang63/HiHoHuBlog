using HiHoHuBlog.Modules.Search.Entity;
using HiHoHuBlog.Modules.Search.Repository.Interface;
using HiHoHuBlog.Utils;

namespace HiHoHuBlog.Modules.Search.Repository.Implementation;

public class EsMigrationRepository(EsClient client) : IMigrationRepository
{
    public async Task<Result<Unit, Err>> AddBulkBlogAsync(IEnumerable<BlogSearchDoc> blogs, DateTime? date)
    {
        var r = await client.Client.BulkAsync(b => b.Index<BlogSearchDoc>().IndexMany(blogs));

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

    public async Task<Result<Unit, Err>> AddBulkTagAsync(IEnumerable<TagSearchDoc> tags, DateTime? date)
    {
        var r = await client.Client.BulkAsync(b => b.Index<TagSearchDoc>().IndexMany(tags));

        if (!r.IsValid)
        {
            return Result<Unit, Err>.Err(UtilErrors.InternalServerError(r.OriginalException));
        }

        var migrationTimeDoc = new MigrationTimestamp()
        {
            Timestamp = date ?? DateTime.Now,
            Index = "tag_search"
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

    public async Task<Result<Unit, Err>> AddBulkUserAsync(IEnumerable<UserSearchDoc> users, DateTime? date)
    {
        var r = await client.Client.BulkAsync(b => b.Index<UserSearchDoc>().IndexMany(users));

        if (!r.IsValid)
        {
            return Result<Unit, Err>.Err(UtilErrors.InternalServerError(r.OriginalException));
        }

        var migrationTimeDoc = new MigrationTimestamp()
        {
            Timestamp = date ?? DateTime.Now,
            Index = "user_search"
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

    public async Task<Result<MigrationTimestamp?, Err>> GetLastMigrateTime(string index)
    {
        var searchResponse = await client.Client.SearchAsync<MigrationTimestamp>(s => s
            .Index<MigrationTimestamp>()
            .Query(q => q
                .Term(t => t
                    .Field(f => f.Index ).Value(index)
                )
            )
            .Sort(sort => sort
                .Descending(p => p.Timestamp) 
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