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


    public async Task<Result<MigrationTimestamp?, Err>> GetLastMigrateTime()
    {
        var searchResponse = await client.Client.SearchAsync<MigrationTimestamp>(s => s
            .Index<MigrationTimestamp>()
            .Query(q => q
                .Term(t => t
                    .Field(f => f.Index ).Value("blog_search")
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

    public async Task<Result<IEnumerable<BlogSearchDoc>?, Err>> SearchBlog(string arg , Paging paging)
    {
        var searchResponse = await client.Client.SearchAsync<BlogSearchDoc>(s => s
            .From(paging.GetOffSet())  
            .Size(paging.PageSize)    
            .Query(q => q
                .Bool(b => b
                    .Should(sh => sh
                            .MultiMatch(mm => mm
                                .Query(arg)
                                .Fields(f => f
                                    .Field("title")
                                    .Field("content")
                                )
                                .Fuzziness(Fuzziness.Auto)
                                .Type(TextQueryType.BestFields)
                                .Boost(1.0)
                            ),
                        sh => sh
                            .MultiMatch(mm => mm
                                .Query(arg)  // Fixed query here
                                .Fields(f => f
                                    .Field("title^2")
                                    .Field("content")
                                )
                                .Operator(Operator.Or)
                                .Type(TextQueryType.PhrasePrefix)
                                .Boost(2.0)
                            )
                    )
                    .Filter(f => f
                            .Term(t => t.Field("isPublished").Value(true)),
                        f => f
                            .Term(t => t.Field("status").Value(1))
                    )
                )
            )
        );

        if (searchResponse.IsValid && searchResponse.Documents.Any())
        {
            paging.Total = (int)searchResponse.Total;

            return Result<IEnumerable<BlogSearchDoc>?, Err>.Ok(searchResponse.Documents);
        }
    
        return Result<IEnumerable<BlogSearchDoc>?, Err>.Err(UtilErrors.InternalServerError(searchResponse.OriginalException));
    }

}