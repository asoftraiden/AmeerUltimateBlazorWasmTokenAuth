namespace UltimateBlazorWasmTokenAuth.Contracts;

public interface IRepositoryManager
{
    ICompanyRepository Company { get; }
    INonConformingRepository NonConforming { get; }
    IPartRepository Part { get; }
    ICellRepository Cell { get; }
    IModelRepository Model { get; }

}