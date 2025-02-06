using UltimateBlazorWasmTokenAuth.DataTransferObjects;

namespace UltimateBlazorWasmTokenAuth.Contracts;

public interface INonConformingRepository
{
    Task<(HttpResponseMessage, NonConformingDto, Dictionary<string, object>?)> CreateNonConformingAsync(NonConformingForCreationDto newNonConforming);
}
