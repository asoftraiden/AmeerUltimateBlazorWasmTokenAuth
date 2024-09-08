using UltimateBlazorWasmTokenAuth.DataTransferObjects;

namespace UltimateBlazorWasmTokenAuth.Contracts;

public interface ICompanyRepository
{
    Task<(HttpResponseMessage, CompanyDto)> CreateCompanyAsync(CompanyForCreationDto newCompany);
    public Task<(HttpResponseMessage, List<CompanyDto>)> GetCompaniesAsync();
}