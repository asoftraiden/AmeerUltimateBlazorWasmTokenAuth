using Microsoft.AspNetCore.JsonPatch;
using UltimateBlazorWasmTokenAuth.DataTransferObjects;
using UltimateBlazorWasmTokenAuth.Repository;

namespace UltimateBlazorWasmTokenAuth.Contracts;

public interface IModelRepository
{
    Task<(HttpResponseMessage, ModelDto)> GetModelAsync(Guid modelId);
    Task<(HttpResponseMessage, ModelDto, Dictionary<string, object>?)> CreateModelAsync(ModelForCreationDto newModel);
    Task<(HttpResponseMessage, Dictionary<string, object>)> UpdateModelAsync(Guid modelId, ModelForUpdateDto model);
    Task<(HttpResponseMessage, Dictionary<string, object>)> DeleteModelAsync(Guid modelId);
    Task<(HttpResponseMessage, List<ModelDto>, PaginationMetadata)> GetModelsAsync(int pageNumber = 1, int pageSize = 10, string searchTerm = null, string sortBy = null, string sortOrder = null);
    Task<(HttpResponseMessage, Dictionary<string, object>)> PatchModelAsync(Guid modelId, JsonPatchDocument patchDoc);
}
