using Nest;

namespace HiHoHuBlog;

public class EsClient
{
    private readonly ElasticClient _client; 
    public EsClient(IConfiguration configuration)
    {
        var userName  = configuration["ElasticSearch:UserName"];
        var password = configuration["ElasticSearch:Password"];
        var url = configuration["ElasticSearch:Uri"];
        
        var settings = new ConnectionSettings(new Uri(url))
            .ServerCertificateValidationCallback((sender, cert, chain, errors) => true)
            .BasicAuthentication(userName,password);

        _client = new ElasticClient(settings);

        var pingResult =  _client.Ping();
        Console.WriteLine(pingResult.IsValid);
        Console.WriteLine(pingResult.DebugInformation);
    }
    
    public ElasticClient Client => _client;
}