using HiHoHuBlog.Modules.Blog.Entity;
using HiHoHuBlog.Modules.Blog.Repository;
using HiHoHuBlog.Modules.Search.Entity;
using HiHoHuBlog.Modules.Search.Repository.Interface;
using HiHoHuBlog.Utils;
using Nest;

namespace HiHoHuBlog.Modules.Search.Repository.Implementation;

public class EsSearchBlogRepository(EsClient client, IUserBlogActionRepository userBlogRepo) : ISearchBlogRepository
{
    
    private readonly IUserBlogActionRepository _userBlogRepo = userBlogRepo;
    public async Task<Result<Unit, Err>> AddBulkAsync(IEnumerable<BlogSearchDoc> blogs, DateTime? date)
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

    public async Task<Result<IEnumerable<BlogSearchDoc>?, Err>> SearchBlog(string arg, Paging paging)
    {
        var searchResponse = await client.Client.SearchAsync<BlogSearchDoc>(s => s
            .From(paging.GetOffSet() - 1)
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
                                .Query(arg) // Fixed query here
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

        return Result<IEnumerable<BlogSearchDoc>?, Err>.Err(
            UtilErrors.InternalServerError(searchResponse.OriginalException));
    }

    public async Task<Result<IEnumerable<BlogSearchDoc>?, Err>> AdminSearchBlog(string arg, BlogFilter? filter,
        Paging paging)
    {
        var filters = new List<Func<QueryContainerDescriptor<BlogSearchDoc>, QueryContainer>>();
        if (filter != null)
        {
            if (filter.UserId is not null)
            {
                filters.Add(f => f.Term(t => t.Field("userId").Value(filter.UserId)));
            }

            if (filter.Status is not null)
            {
                filters.Add(f => f.Terms(t => t.Field("status").Terms(filter.Status)));
            }

            if (filter.IsPublished is not null)
            {
                filters.Add(f => f.Term(t => t.Field("isPublished").Value(filter.IsPublished)));
            }

            if (filter.LtCreatedAt is not null)
            {
                filters.Add(f => f.DateRange(d => d.Field("createdAt").LessThanOrEquals(filter.LtCreatedAt)));
            }

            if (filter.GtCreatedAt is not null)
            {
                filters.Add(f => f.DateRange(d => d.Field("createdAt").GreaterThanOrEquals(filter.GtCreatedAt)));
            }

            if (filter.LtUpdatedAt is not null)
            {
                filters.Add(f => f.DateRange(d => d.Field("updatedAt").LessThanOrEquals(filter.LtUpdatedAt)));
            }

            if (filter.GtUpdatedAt is not null)
            {
                filters.Add(f => f.DateRange(d => d.Field("updatedAt").GreaterThanOrEquals(filter.GtUpdatedAt)));
            }
        }

        var searchResponse = await client.Client.SearchAsync<BlogSearchDoc>(s => s
            .From(paging.GetOffSet())
            .Size(paging.PageSize)
            .Query(q => q
                .Bool(b => b
                    .Must(sh => sh
                        .MultiMatch(mm => mm
                            .Query(arg)
                            .Fields(f => f
                                .Field("title")
                                .Field("content")
                                .Field("user.userName")
                            )
                            .Operator(Operator.Or)
                            .Type(TextQueryType.PhrasePrefix)
                        )
                    )
                    .Filter(filters)
                )
            )
        );

        if (searchResponse.IsValid && searchResponse.Documents.Any())
        {
            paging.Total = (int)searchResponse.Total;
            return Result<IEnumerable<BlogSearchDoc>?, Err>.Ok(searchResponse.Documents);
        }

        return Result<IEnumerable<BlogSearchDoc>?, Err>.Err(
            UtilErrors.InternalServerError(searchResponse.OriginalException));
    }

    public async Task<Result<IEnumerable<BlogSearchDoc>?, Err>> RecommendSearchBlogByUser(IRequester requester,int seed, Paging paging)
    {
        var history = await _userBlogRepo.ListReadHistory(requester.GetId(),new Paging(1, 2));

        if (!history.IsOk || history.Value is null )
        {
            return await RandomBlog(seed,paging);
        }
        
        var docs = await client.Client.SearchAsync<BlogSearchDoc>(s => s
            .Size(paging.PageSize)
            .From(paging.GetOffSet())
            .Query(q => q
                .Bool(b => b
                    .Must(
                        qq => qq.FunctionScore(f => f
                            .Query(mlt => mlt
                                .MoreLikeThis(m => m
                                    .Fields(ff => ff.Field("title").Field("content"))
                                    .Like(l =>
                                    {
                                        foreach (var u in history.Value)
                                        {
                                            l.Document(d => d.Id(u?.Blog?.Id));
                                        }
                                        return l;
                                    })
                                    .MinTermFrequency(1)
                                    .MinDocumentFrequency(1)
                                    .MaxQueryTerms(25)
                                )
                            )
                            .Functions(ff => ff
                                .RandomScore(ss => ss
                                        .Seed(seed)
                                        .Weight(2) 
                                )
                            )
                        )
                    )
                    .Filter(f => f
                            .Term(t => t.Field("isPublished").Value(true)),
                        f => f
                            .Term(t => t.Field("status").Value(1))
                    )
                )
            )
            .Sort(ss => ss.Descending(SortSpecialField.Score))
        );
        if (docs.IsValid && docs.Documents.Any())
        {
            paging.Total = (int)docs.Total;
            return Result<IEnumerable<BlogSearchDoc>?, Err>.Ok(docs.Documents);
        }

        return Result<IEnumerable<BlogSearchDoc>?, Err>.Err(UtilErrors.InternalServerError(docs.OriginalException));
    }

    public async Task<Result<IEnumerable<BlogSearchDoc>?, Err>> RecommendSearchBlogByBlog(IRequester? requester, int id,
        Paging paging)
    {
        var docs = await client.Client.SearchAsync<BlogSearchDoc>(s => s
            .Size(paging.PageSize)
            .From(paging.GetOffSet() )
            .Query(q => q
                .Bool(b => b
                    .Must(sh => sh
                        .MoreLikeThis(m => m
                            .Fields(f => f.Field("title").Field("content"))
                            .Like(l => l
                                .Document(d => d
                                    .Id(id)))
                            .MinTermFrequency(1)
                            .MinDocumentFrequency(1)
                            .MaxQueryTerms(25)))
                    .Filter(f => f
                            .Term(t => t.Field("isPublished").Value(true)),
                        f => f
                            .Term(t => t.Field("status").Value(1))
                    )
                )));

        if (docs.IsValid && docs.Documents.Any())
        {
            paging.Total = (int)docs.Total;
            return Result<IEnumerable<BlogSearchDoc>?, Err>.Ok(docs.Documents);
        }

        return Result<IEnumerable<BlogSearchDoc>?, Err>.Err(UtilErrors.InternalServerError(docs.OriginalException));
    }

    public async Task<Result<IEnumerable<BlogSearchDoc>?, Err>> RandomBlog(int seed, Paging paging)
    {
        var docs = await client.Client.SearchAsync<BlogSearchDoc>(s => s
            .Size(paging.PageSize)
            .From(paging.GetOffSet())
            .Query(q => q
                .Bool(b => b
                    .Must(mm => mm
                        .FunctionScore(f => f
                            .Functions(ff => ff
                                .RandomScore(ss => ss
                                    .Seed(seed)))))
                    .Filter(f => f
                            .Term(t => t.Field("isPublished").Value(true)),
                        f => f
                            .Term(t => t.Field("status").Value(1))
                    )))
        );

        if (docs.IsValid && docs.Documents.Any())
        {
            paging.Total = (int)docs.Total;
            return Result<IEnumerable<BlogSearchDoc>?, Err>.Ok(docs.Documents);
        }

        return Result<IEnumerable<BlogSearchDoc>?, Err>.Err(UtilErrors.InternalServerError(docs.OriginalException));
    }
}