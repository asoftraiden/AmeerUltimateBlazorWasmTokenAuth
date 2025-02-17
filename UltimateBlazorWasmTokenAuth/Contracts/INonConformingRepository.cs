using UltimateBlazorWasmTokenAuth.DataTransferObjects;
using UltimateBlazorWasmTokenAuth.Repository;

namespace UltimateBlazorWasmTokenAuth.Contracts;

public interface INonConformingRepository
{
    Task<(HttpResponseMessage, NonConformingDto, Dictionary<string, object>?)> CreateNonConformingAsync(NonConformingForCreationDto newNonConforming);
    Task<(HttpResponseMessage, Dictionary<string, object>)> DeleteNonConformingAsync(Guid id);
    Task<(HttpResponseMessage, NonConformingDto, Dictionary<string, object>?)> GetNonConformingAsync(Guid id);
    Task<(HttpResponseMessage, List<NonConformingDto>, PaginationMetadata)> GetNonConformingsAsync(int pageNumber = 1, int pageSize = 10, string searchTerm = null, string sortBy = null, string sortOrder = null);
    Task<(HttpResponseMessage, Dictionary<string, object>)> UpdateNonConformingAsync(Guid id, NonConformingForUpdateDto nonConforming);
}
