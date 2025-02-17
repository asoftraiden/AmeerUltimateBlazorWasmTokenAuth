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

    public async Task<(HttpResponseMessage, List<NonConformingDto>, PaginationMetadata)> GetNonConformingsAsync(int pageNumber = 1, int pageSize = 10, string searchTerm = null, string sortBy = null, string sortOrder = null)
    {
        var endpoint = $"api/nonconformings?pageNumber={pageNumber}&pageSize={pageSize}";

        if (!string.IsNullOrEmpty(searchTerm))
        {
            endpoint += $"&searchterm={Uri.EscapeDataString(searchTerm)}";
        }

        if (!string.IsNullOrEmpty(sortBy))
        {
            endpoint += $"&orderby={sortBy}";
            if (!string.IsNullOrEmpty(sortOrder))
            {
                endpoint += $" {sortOrder}";
            }
        }

        var response = await _client.GetAsync(endpoint);
        var responseJson = await response?.Content.ReadAsStringAsync();
        var nonconformings = JsonConvert.DeserializeObject<List<NonConformingDto>>(responseJson);
        PaginationMetadata paginationMetadata = null;

        if (response.Headers.TryGetValues("X-Pagination", out var paginationHeader))
        {
            var paginationValue = paginationHeader.FirstOrDefault();
            if (!string.IsNullOrEmpty(paginationValue))
            {
                paginationMetadata = JsonConvert.DeserializeObject<PaginationMetadata>(paginationValue);
            }
        }

        return (response, nonconformings, paginationMetadata);
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

    public async Task<(HttpResponseMessage, Dictionary<string, object>)> UpdateNonConformingAsync(Guid id, NonConformingForUpdateDto nonConforming)
    {
        var endpoint = $"api/nonconformings/{id}";
        var json = JsonConvert.SerializeObject(nonConforming);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _client.PutAsync(endpoint, content);
        var responseJson = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            return (response, null);
        }
        else
        {
            var errors = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseJson);
            return (response, errors);
        }
    }

    public async Task<(HttpResponseMessage, Dictionary<string, object>)> DeleteNonConformingAsync(Guid id)
    {
        var endpoint = $"api/nonconformings/{id}";
        var response = await _client.DeleteAsync(endpoint);
        var responseJson = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            return (response, null);
        }
        else
        {
            var errors = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseJson);
            return (response, errors);
        }
    }

    public async Task<(HttpResponseMessage, NonConformingDto, Dictionary<string, object>?)> GetNonConformingAsync(Guid id)
    {
        var endpoint = $"api/nonconformings/{id}";
        var response = await _client.GetAsync(endpoint);
        var responseJson = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            var nonConforming = JsonConvert.DeserializeObject<NonConformingDto>(responseJson);
            return (response, nonConforming, null);
        }
        else
        {
            var errors = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseJson);
            return (response, null, errors);
        }
    }

}