using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using UltimateBlazorWasmTokenAuth.Contracts;
using UltimateBlazorWasmTokenAuth.DataTransferObjects;

namespace UltimateBlazorWasmTokenAuth.Repository;

public class ModelRepository : IModelRepository
{
    private readonly IHttpClientFactory _factory;
    private readonly HttpClient _client;

    public ModelRepository(IHttpClientFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient("ServerApi");
        _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<(HttpResponseMessage, List<ModelDto>, PaginationMetadata)> GetModelsAsync(int pageNumber = 1, int pageSize = 10, string searchTerm = null, string sortBy = null, string sortOrder = null)
    {
        var endpoint = $"api/models?pageNumber={pageNumber}&pageSize={pageSize}";
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
        var models = JsonConvert.DeserializeObject<List<ModelDto>>(responseJson);
        PaginationMetadata paginationMetadata = null;

        if (response.Headers.TryGetValues("X-Pagination", out var paginationHeader))
        {
            var paginationValue = paginationHeader.FirstOrDefault();
            if (!string.IsNullOrEmpty(paginationValue))
            {
                paginationMetadata = JsonConvert.DeserializeObject<PaginationMetadata>(paginationValue);
            }
        }

        return (response, models, paginationMetadata);
    }

    public async Task<(HttpResponseMessage, ModelDto)> GetModelAsync(Guid modelId)
    {
        var endpoint = $"api/models/{modelId}";
        var response = await _client.GetAsync(endpoint);
        var responseJson = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            var model = JsonConvert.DeserializeObject<ModelDto>(responseJson);
            return (response, model);
        }

        return (response, null);
    }

    public async Task<(HttpResponseMessage, ModelDto, Dictionary<string, object>?)> CreateModelAsync(ModelForCreationDto newModel)
    {
        var endpoint = "api/models";
        var json = JsonConvert.SerializeObject(newModel);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _client.PostAsync(endpoint, content);
        var responseJson = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            var createdModel = JsonConvert.DeserializeObject<ModelDto>(responseJson);
            return (response, createdModel, null);
        }
        else
        {
            var errors = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseJson);
            return (response, null, errors);
        }
    }

    public async Task<(HttpResponseMessage, Dictionary<string, object>)> UpdateModelAsync(Guid modelId, ModelForUpdateDto model)
    {
        var endpoint = $"api/models/{modelId}";
        var json = JsonConvert.SerializeObject(model);
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

    public async Task<(HttpResponseMessage, Dictionary<string, object>)> DeleteModelAsync(Guid modelId)
    {
        var endpoint = $"api/models/{modelId}";
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

    public async Task<(HttpResponseMessage, Dictionary<string, object>)> PatchModelAsync(Guid modelId, JsonPatchDocument patchDoc)
    {
        var endpoint = $"api/models/{modelId}";
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