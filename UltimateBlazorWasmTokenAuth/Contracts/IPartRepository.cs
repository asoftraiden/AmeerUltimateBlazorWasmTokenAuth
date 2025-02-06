using Microsoft.AspNetCore.JsonPatch;
using UltimateBlazorWasmTokenAuth.DataTransferObjects;
using UltimateBlazorWasmTokenAuth.Repository;

namespace UltimateBlazorWasmTokenAuth.Contracts;

public interface IPartRepository
{
    Task<(HttpResponseMessage, PartDto)> GetPartAsync(Guid supplierId, Guid partId);
    Task<(HttpResponseMessage, PartDto, Dictionary<string, object>?)> CreatePartAsync(Guid supplierId, PartForCreationDto newPart);
    Task<(HttpResponseMessage, Dictionary<string, object>)> UpdatePartAsync(Guid supplierId, Guid partId, PartForUpdateDto part);
    Task<(HttpResponseMessage, Dictionary<string, object>)> DeletePartAsync(Guid supplierId, Guid partId);
    Task<(HttpResponseMessage, List<PartDto>, PaginationMetadata)> GetPartsAsync(int pageNumber = 1, int pageSize = 10, string searchTerm = null, string sortBy = null, string sortOrder = null);
    Task<(HttpResponseMessage, Dictionary<string, object>)> PatchPartAsync(Guid supplierId, Guid partId, JsonPatchDocument patchDoc);
}
