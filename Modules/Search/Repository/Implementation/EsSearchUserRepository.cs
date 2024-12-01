using HiHoHuBlog.Modules.Search.Entity;
using HiHoHuBlog.Modules.Search.Repository.Interface;
using HiHoHuBlog.Utils;
using Nest;

namespace HiHoHuBlog.Modules.Search.Repository.Implementation;

public class EsSearchUserRepository(EsClient client)  : ISearchUserRepository
{
    public async Task<Result<IEnumerable<UserSearchDoc>?, Err>> SearchUsers(string arg, Paging paging)
    {
        var searchResponse = await client.Client.SearchAsync<UserSearchDoc>(s => s
            .From(paging.GetOffSet())  
            .Size(paging.PageSize)    
            .Query(q => q
                .Bool(b => b
                    .Must(sh => sh
                            .MultiMatch(mm => mm
                                .Query(arg)
                                .Fields(f => f
                                    .Field("userName")
                                    .Field("lastName")
                                    .Field("firstName")
                                )
                                .Operator(Operator.Or)
                                .Type(TextQueryType.PhrasePrefix)
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

            return Result<IEnumerable<UserSearchDoc>?, Err>.Ok(searchResponse.Documents);
        }
    
        return Result<IEnumerable<UserSearchDoc>?, Err>.Err(UtilErrors.InternalServerError(searchResponse.OriginalException));
    }

    public async Task<Result<IEnumerable<UserSearchDoc>?, Err>> RandomUsers(int seed, Paging paging)
    {
        var docs = await client.Client.SearchAsync<UserSearchDoc>(s => s
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
            return Result<IEnumerable<UserSearchDoc>?, Err>.Ok(docs.Documents);
        }

        return Result<IEnumerable<UserSearchDoc>?, Err>.Err(UtilErrors.InternalServerError(docs.OriginalException));
    }

    public Task<Result<IEnumerable<UserSearchDoc>?, Err>> RecommendUsers(IRequester requester, Paging paging)
    {
        throw new NotImplementedException();
    }
}