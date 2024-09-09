using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using UltimateBlazorWasmTokenAuth.Contracts;
using UltimateBlazorWasmTokenAuth.DataTransferObjects;

namespace UltimateBlazorWasmTokenAuth.Repository;

public class CompanyRepository : ICompanyRepository
{
    private readonly IHttpClientFactory _factory;
    private readonly HttpClient _client;

    public CompanyRepository(IHttpClientFactory factory)
    {
        _factory = factory;
        _client  = _factory.CreateClient("ServerApi");
        _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

    }

    public async Task<(HttpResponseMessage, CompanyDto)> CreateCompanyAsync(CompanyForCreationDto newCompany)
    {
        var endpoint = "api/companies";
        var json = JsonConvert.SerializeObject(newCompany);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _client.PostAsync(endpoint, content);
        // response.EnsureSuccessStatusCode();
        var responseJson = await response?.Content.ReadAsStringAsync();
        var createdCompany = JsonConvert.DeserializeObject<CompanyDto>(responseJson);
        return (response, createdCompany);
    }

    public async Task<(HttpResponseMessage, List<CompanyDto>)> GetCompaniesAsync()
    {
        var endpoint = "api/companies";
        var response = await _client.GetAsync(endpoint);
        //response.EnsureSuccessStatusCode();
        var responseJson = await response?.Content.ReadAsStringAsync();
        var companies = JsonConvert.DeserializeObject<List<CompanyDto>>(responseJson);
        return (response, companies);
    }

}