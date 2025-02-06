using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using UltimateBlazorWasmTokenAuth.Contracts;
using UltimateBlazorWasmTokenAuth.DataTransferObjects;

namespace UltimateBlazorWasmTokenAuth.Repository;

public class PartRepository : IPartRepository
{
    private readonly IHttpClientFactory _factory;
    private readonly HttpClient _client;

    public PartRepository(IHttpClientFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient("ServerApi");
        _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

   
    public async Task<(HttpResponseMessage, List<PartDto>, PaginationMetadata)> GetPartsAsync(int pageNumber = 1, int pageSize = 10, string searchTerm = null, string sortBy = null, string sortOrder = null)
    {
        var endpoint = $"api/parts?pageNumber={pageNumber}&pageSize={pageSize}";
        if (!string.IsNullOrEmpty(searchTerm))
        {
            endpoint += $"&searchTerm={Uri.EscapeDataString(searchTerm)}";
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
        var responseJson = await response.Content.ReadAsStringAsync();
        var parts = JsonConvert.DeserializeObject<List<PartDto>>(responseJson);
        PaginationMetadata paginationMetadata = null;
        if (response.Headers.TryGetValues("X-Pagination", out var paginationHeader))
        {
            var paginationValue = paginationHeader.FirstOrDefault();
            if (!string.IsNullOrEmpty(paginationValue))
            {
                paginationMetadata = JsonConvert.DeserializeObject<PaginationMetadata>(paginationValue);
            }
        }

        return (response, parts, paginationMetadata);
    }




    public async Task<(HttpResponseMessage, PartDto)> GetPartAsync(Guid supplierId, Guid partId)
    {
        var endpoint = $"api/suppliers/{supplierId}/parts/{partId}";
        var response = await _client.GetAsync(endpoint);
        var responseJson = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            var part = JsonConvert.DeserializeObject<PartDto>(responseJson);
            return (response, part);
        }

        return (response, null);
    }

    public async Task<(HttpResponseMessage, PartDto, Dictionary<string, object>?)> CreatePartAsync(Guid supplierId, PartForCreationDto newPart)
    {
        var endpoint = $"api/suppliers/{supplierId}/parts";
        var json = JsonConvert.SerializeObject(newPart);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _client.PostAsync(endpoint, content);
        var responseJson = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            var createdPart = JsonConvert.DeserializeObject<PartDto>(responseJson);
            return (response, createdPart, null);
        }
        else
        {
            var errors = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseJson);
            return (response, null, errors);
        }
    }

    public async Task<(HttpResponseMessage, Dictionary<string, object>)> UpdatePartAsync(Guid supplierId, Guid partId, PartForUpdateDto part)
    {
        var endpoint = $"api/suppliers/{supplierId}/parts/{partId}";
        var json = JsonConvert.SerializeObject(part);
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

    public async Task<(HttpResponseMessage, Dictionary<string, object>)> DeletePartAsync(Guid supplierId, Guid partId)
    {
        var endpoint = $"api/suppliers/{supplierId}/parts/{partId}";
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
    public async Task<(HttpResponseMessage, Dictionary<string, object>)> PatchPartAsync(Guid supplierId, Guid partId, JsonPatchDocument patchDoc)
    {
        var endpoint = $"api/suppliers/{supplierId}/parts/{partId}";
        var json = JsonConvert.SerializeObject(patchDoc);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _client.PatchAsync(endpoint, content);
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
}