using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using UltimateBlazorWasmTokenAuth.Contracts;
using UltimateBlazorWasmTokenAuth.DataTransferObjects;

namespace UltimateBlazorWasmTokenAuth.Repository;

public class NonConformingRepository : INonConformingRepository
{
    private readonly IHttpClientFactory _factory;
    private readonly HttpClient _client;

    public NonConformingRepository(IHttpClientFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient("ServerApi");
        _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<(HttpResponseMessage, NonConformingDto, Dictionary<string, object>?)> CreateNonConformingAsync(NonConformingForCreationDto newNonConforming)
    {
        var endpoint = "api/nonconformings";
        var json = JsonConvert.SerializeObject(newNonConforming);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _client.PostAsync(endpoint, content);
        var responseJson = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            var createdNonConforming = JsonConvert.DeserializeObject<NonConformingDto>(responseJson);
            return (response, createdNonConforming, null);
        }
        else
        {
            var errors = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseJson);
            return (response, null, errors);
        }
    }
}