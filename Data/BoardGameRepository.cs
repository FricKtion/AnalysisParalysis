using System.Security;
using System.Xml.Serialization;
using AnalysisParalysis.Data.Definitions;
using AnalysisParalysis.Data.Models.BoardGameGeek;
using AnalysisParalysis.Services.Definitions;

namespace AnalysisParalysis.Data;

/// <inheritdoc />
public class BoardGameRepository : IBoardGameRepository
{
    private readonly IAppSettingService _appSettingService;

    private readonly HttpClient _httpClient;

    public BoardGameRepository(IAppSettingService appSettings,  IHttpClientFactory httpClientFactory)
        => (_appSettingService, _httpClient) = (appSettings, httpClientFactory.CreateClient());

    /// <inheritdoc />
    public async Task<Collection?> GetCollection(string bggUserName)
    {
        var baseUrl = _appSettingService.Setting<string>("GameApiBaseUrl");
        var maxRetry = _appSettingService.Setting<int>("MaxRetry");
        
        var collectionEndpoint = $"{baseUrl}/collection"
            + $"?username={bggUserName}"
            + "&excludesubtype=boardgameexpansion"
            + "&own=1";

        var apiResponse = await CallBoardGameGeek(collectionEndpoint, maxRetry);

        if(!apiResponse.IsSuccessStatusCode)
            return null;

        return await ParseApiResponse<Collection>(apiResponse);
    }

    /// <inheritdoc />
    public async Task<Thing?> GetBoardGameDetails(IEnumerable<int> boardGameIds)
    {
        var baseUrl = _appSettingService.Setting<string>("GameApiBaseUrl");
        var maxRetry = _appSettingService.Setting<int>("MaxRetry");

        var gameEndpoint = $"{baseUrl}/thing"
            + "?type=boardgame"
            + $"&id={string.Join(',', boardGameIds)}";

        var apiResponse = await CallBoardGameGeek(gameEndpoint, maxRetry);

        if(!apiResponse.IsSuccessStatusCode)
            return null;
            
        return await ParseApiResponse<Thing>(apiResponse);
    }

    private async Task<HttpResponseMessage> CallBoardGameGeek(string endpoint, int maxRetry)
    {
        var apiResponse = await _httpClient.GetAsync(endpoint);

        var attempts = 0;
        while(apiResponse.StatusCode == System.Net.HttpStatusCode.Accepted
            && attempts <= maxRetry)
        {
            await Task.Delay(3000);

            apiResponse = await _httpClient.GetAsync(endpoint);

            attempts++;
        }

        return apiResponse;
    }

    /// <summary>
    /// Parses a response from the BGG API and attempts to serialize it into a C# object.
    /// </summary>
    /// <typeparam name="T">The type to serialize to.</typeparam>
    /// <param name="apiResponse">BGG API response.</param>
    /// <returns>The serialized object.</returns>
    private async Task<T> ParseApiResponse<T>(HttpResponseMessage apiResponse)
    {
        var responseContent = await apiResponse.Content.ReadAsStreamAsync();
        var serializer = new XmlSerializer(typeof(T));

        return (T)(serializer.Deserialize(responseContent) ?? default(T)!);
    }
}
