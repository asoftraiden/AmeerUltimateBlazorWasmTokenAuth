using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using UltimateBlazorWasmTokenAuth.Contracts;
using UltimateBlazorWasmTokenAuth.DataTransferObjects;


namespace UltimateBlazorWasmTokenAuth.Repository;

public class CellRepository : ICellRepository
{
    private readonly IHttpClientFactory _factory;
    private readonly HttpClient _client;

    public CellRepository(IHttpClientFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient("ServerApi");
        _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<(HttpResponseMessage, List<CellDto>, PaginationMetadata)> GetCellsAsync(int pageNumber = 1, int pageSize = 10, string searchTerm = null, string sortBy = null, string sortOrder = null)
    {
        var endpoint = $"api/cells?pageNumber={pageNumber}&pageSize={pageSize}";
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
        var cells = JsonConvert.DeserializeObject<List<CellDto>>(responseJson);
        PaginationMetadata paginationMetadata = null;

        if (response.Headers.TryGetValues("X-Pagination", out var paginationHeader))
        {
            var paginationValue = paginationHeader.FirstOrDefault();
            if (!string.IsNullOrEmpty(paginationValue))
            {
                paginationMetadata = JsonConvert.DeserializeObject<PaginationMetadata>(paginationValue);
            }
        }

        return (response, cells, paginationMetadata);
    }

    public async Task<(HttpResponseMessage, CellDto)> GetCellAsync(Guid cellId)
    {
        var endpoint = $"api/cells/{cellId}";
        var response = await _client.GetAsync(endpoint);
        var responseJson = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            var cell = JsonConvert.DeserializeObject<CellDto>(responseJson);
            return (response, cell);
        }

        return (response, null);
    }

    public async Task<(HttpResponseMessage, CellDto, Dictionary<string, object>?)> CreateCellAsync(CellForCreationDto newCell)
    {
        var endpoint = "api/cells";
        var json = JsonConvert.SerializeObject(newCell);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _client.PostAsync(endpoint, content);
        var responseJson = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            var createdCell = JsonConvert.DeserializeObject<CellDto>(responseJson);
            return (response, createdCell, null);
        }
        else
        {
            var errors = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseJson);
            return (response, null, errors);
        }
    }

    public async Task<(HttpResponseMessage, Dictionary<string, object>)> UpdateCellAsync(Guid cellId, CellForUpdateDto cell)
    {
        var endpoint = $"api/cells/{cellId}";
        var json = JsonConvert.SerializeObject(cell);
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

    public async Task<(HttpResponseMessage, Dictionary<string, object>)> DeleteCellAsync(Guid cellId)
    {
        var endpoint = $"api/cells/{cellId}";
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

    public async Task<(HttpResponseMessage, Dictionary<string, object>)> PatchCellAsync(Guid cellId, JsonPatchDocument patchDoc)
    {
        var endpoint = $"api/cells/{cellId}";
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