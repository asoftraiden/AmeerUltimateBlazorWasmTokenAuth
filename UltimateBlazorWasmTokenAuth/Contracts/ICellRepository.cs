using Microsoft.AspNetCore.JsonPatch;
using UltimateBlazorWasmTokenAuth.DataTransferObjects;
using UltimateBlazorWasmTokenAuth.Repository;

namespace UltimateBlazorWasmTokenAuth.Contracts;


public interface ICellRepository
{
    Task<(HttpResponseMessage, CellDto)> GetCellAsync(Guid cellId);
    Task<(HttpResponseMessage, CellDto, Dictionary<string, object>?)> CreateCellAsync(CellForCreationDto newCell);
    Task<(HttpResponseMessage, Dictionary<string, object>)> UpdateCellAsync(Guid cellId, CellForUpdateDto cell);
    Task<(HttpResponseMessage, Dictionary<string, object>)> DeleteCellAsync(Guid cellId);
    Task<(HttpResponseMessage, List<CellDto>, PaginationMetadata)> GetCellsAsync(int pageNumber = 1, int pageSize = 10, string searchTerm = null, string sortBy = null, string sortOrder = null);
    Task<(HttpResponseMessage, Dictionary<string, object>)> PatchCellAsync(Guid cellId, JsonPatchDocument patchDoc);
}