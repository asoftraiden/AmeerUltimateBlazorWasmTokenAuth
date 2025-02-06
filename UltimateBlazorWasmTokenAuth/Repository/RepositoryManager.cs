using UltimateBlazorWasmTokenAuth.Contracts;

namespace UltimateBlazorWasmTokenAuth.Repository;

public class RepositoryManager : IRepositoryManager
{
    private readonly Lazy<ICompanyRepository> _companyRepository;
    private readonly Lazy<INonConformingRepository> _nonConformingRepository;
    private readonly Lazy<IPartRepository> _partRepository;
    private readonly Lazy<ICellRepository> _cellRepository;
    private readonly Lazy<IModelRepository> _modelRepository;

    public RepositoryManager(IHttpClientFactory client)
    {
        _companyRepository = new Lazy<ICompanyRepository>(() => new CompanyRepository(client));
        _nonConformingRepository = new Lazy<INonConformingRepository>(() => new NonConformingRepository(client));
        _partRepository = new Lazy<IPartRepository>(() => new PartRepository(client));
        _cellRepository = new Lazy<ICellRepository>(() => new CellRepository(client));
        _modelRepository = new Lazy<IModelRepository>(() => new ModelRepository(client));
    }


    public ICompanyRepository Company => _companyRepository.Value;
    public INonConformingRepository NonConforming => _nonConformingRepository.Value;
    public IPartRepository Part => _partRepository.Value;
    public ICellRepository Cell => _cellRepository.Value;
    public IModelRepository Model => _modelRepository.Value;

}