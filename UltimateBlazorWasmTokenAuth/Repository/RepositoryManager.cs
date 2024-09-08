using UltimateBlazorWasmTokenAuth.Contracts;

namespace UltimateBlazorWasmTokenAuth.Repository;

public class RepositoryManager : IRepositoryManager
{
    private readonly Lazy<ICompanyRepository> _companyRepository;
    public RepositoryManager(IHttpClientFactory client)
    {
        _companyRepository = new Lazy<ICompanyRepository>(() => new CompanyRepository(client));
    }


    public ICompanyRepository Company => _companyRepository.Value;



}