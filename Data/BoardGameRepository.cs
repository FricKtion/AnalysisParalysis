using AnalysisParalysis.Data.Definitions;
using AnalysisParalysis.Data.Models;
using AnalysisParalysis.Services.Definitions;

namespace AnalysisParalysis.Data;

public class BoardGameRepository : IBoardGameRepository
{
    private readonly IAppSettingService _appSettingService;

    private readonly HttpClient _httpClient;

    private readonly int _maxRetry;

    public BoardGameRepository(IAppSettingService appSettings,  IHttpClientFactory httpClientFactory)
        => (_appSettingService, _httpClient) = (appSettings, httpClientFactory.CreateClient());

    public async Task<Collection?> GetCollection(string bggUserName)
    {
        var baseUrl = _appSettingService.Setting<string>("gameApiBaseUrl");
        var maxRetry = _appSettingService.Setting<int>("maxRetry");
        
        var collectionEndpoint = $"{baseUrl}/collection"
            + "?username={bggUserName}"
            + "&excludesubtype=boardgameexpansion"
            + "&own=1";

        var apiResponse = await _httpClient.GetAsync(collectionEndpoint);

        var attempts = 0;
        while(apiResponse.StatusCode == System.Net.HttpStatusCode.Accepted
            && attempts <= maxRetry)
        {
            await Task.Delay(1500);

            apiResponse = await _httpClient.GetAsync(collectionEndpoint);

            attempts++;
        }

        if(!apiResponse.IsSuccessStatusCode)
            return null;

        return ParseApiResponse<Collection>(apiResponse);
    }

    public async Task<BoardGame?> GetBoardGameDetails(int boardGameId)
    {
        var baseUrl = _appSettingService.Setting<string>("gameApiBaseUrl");
        
        var gameEndpoint = $"{baseUrl}/thing"
            + "?type=boardgame";

        var apiResponse = await _httpClient.GetAsync(gameEndpoint);

        if(!apiResponse.IsSuccessStatusCode)
            return null;
            
        return ParseApiResponse<BoardGame>(apiResponse);
    }

    private T ParseApiResponse<T>(HttpResponseMessage apiResponse)
    {
        throw new NotImplementedException();
    }
}
