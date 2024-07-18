using System.Security;
using System.Xml.Serialization;
using AnalysisParalysis.Data.Definitions;
using AnalysisParalysis.Data.Models.BoardGameGeek;
using AnalysisParalysis.Services.Definitions;
using Microsoft.AspNetCore.Server.HttpSys;

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

    /// <summary>
    /// Make a call to the BoardGameGeek XML2 API.
    /// </summary>
    /// <param name="endpoint">The specific endpoint to send the reuqest to.</param>
    /// <param name="maxRetry">The maximum number of attempts to make at this request.</param>
    /// <returns>Returns a successful API response or the latest API response if an OK was never received.</returns>
    /// <exception cref="AggregateException">If no successful attempts an AggregateException is thrown containing all exceptions encountered.</exception>
    private async Task<HttpResponseMessage> CallBoardGameGeek(string endpoint, int maxRetry)
    {
        var exceptions = new List<Exception>();
        for(int i = 0; i < maxRetry; i++)
        {
            try 
            {
                var apiResponse = await _httpClient.GetAsync(endpoint);

                if(apiResponse.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    await Task.Delay(3000);
                    continue;
                }
                    
                return apiResponse;
            }
            catch(HttpRequestException httpEx)
            {
                exceptions.Add(httpEx);
            }
        }

        throw new AggregateException(exceptions);
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
