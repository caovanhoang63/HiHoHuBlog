using HiHoHuBlog.Modules.Search.Entity;
using HiHoHuBlog.Modules.Search.Repository.Interface;
using HiHoHuBlog.Utils;
using Nest;

namespace HiHoHuBlog.Modules.Search.Repository.Implementation;

public class EsSearchTagRepository(EsClient client) : ISearchTagRepository
{
    public async Task<Result<IEnumerable<TagSearchDoc>?, Err>> SearchTags(string arg, Paging paging)
    {
        var searchResponse = await client.Client.SearchAsync<TagSearchDoc>(s => s
            .From(paging.GetOffSet())  
            .Size(paging.PageSize)    
            .Query(q => q
                .Bool(b => b
                    .Should(sh => sh
                            .MultiMatch(mm => mm
                                .Query(arg)
                                .Fields(f => f
                                    .Field("name")
                                )
                                .Fuzziness(Fuzziness.Auto)
                                .Type(TextQueryType.BestFields)
                                .Boost(1.0)
                            ),
                        sh => sh
                            .MultiMatch(mm => mm
                                .Query(arg)
                                .Fields(f => f
                                    .Field("name")
                                )
                                .Operator(Operator.Or)
                                .Type(TextQueryType.PhrasePrefix)
                                .Boost(4.0)
                            )
                    )
                    .Filter(f => f
                            .Term(t => t.Field("status").Value(1))
                    )
                )
            )
        );

        if (searchResponse.IsValid && searchResponse.Documents.Any())
        {
            paging.Total = (int)searchResponse.Total;

            return Result<IEnumerable<TagSearchDoc>?, Err>.Ok(searchResponse.Documents);
        }
    
        return Result<IEnumerable<TagSearchDoc>?, Err>.Err(UtilErrors.InternalServerError(searchResponse.OriginalException));
    }

    public async Task<Result<IEnumerable<TagSearchDoc>?, Err>> SearchTagsForPublish(string arg, Paging paging)
    {
        var searchResponse = await client.Client.SearchAsync<TagSearchDoc>(s => s
            .From(paging.GetOffSet())  
            .Size(paging.PageSize)    
            .Query(q => q
                .Bool(b => b
                    .Must(
                        sh => sh
                            .MultiMatch(mm => mm
                                .Query(arg)
                                .Fields(f => f
                                    .Field("name")
                                )
                                .Operator(Operator.Or)
                                .Type(TextQueryType.PhrasePrefix)
                                .Boost(4.0)
                            )
                    )
                    .Filter(f => f
                        .Term(t => t.Field("status").Value(1))
                    )
                )
            )
        );

        if (searchResponse.IsValid && searchResponse.Documents.Any())
        {
            paging.Total = (int)searchResponse.Total;

            return Result<IEnumerable<TagSearchDoc>?, Err>.Ok(searchResponse.Documents);
        }
    
        return Result<IEnumerable<TagSearchDoc>?, Err>.Err(UtilErrors.InternalServerError(searchResponse.OriginalException));
    }

    public async Task<Result<IEnumerable<TagSearchDoc>?, Err>> RandomSearchTags(int seed, Paging paging)
    {
        var docs = await client.Client.SearchAsync<TagSearchDoc>(s => s
            .Size(paging.PageSize)
            .From(paging.GetOffSet())
            .Query(q => q
                .Bool(b => b
                    .Must(mm => mm
                        .FunctionScore(f => f
                            .Functions(ff => ff
                                .RandomScore(ss => ss
                                    .Seed(seed)))))
                    .Filter(
                        f => f
                            .Term(t => t.Field("status").Value(1))
                    )))
        );

        if (docs.IsValid && docs.Documents.Any())
        {
            paging.Total = (int)docs.Total;
            return Result<IEnumerable<TagSearchDoc>?, Err>.Ok(docs.Documents);
        }

        return Result<IEnumerable<TagSearchDoc>?, Err>.Err(UtilErrors.InternalServerError(docs.OriginalException));
    }
}