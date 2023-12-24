using System;
using System.Net;

namespace ThreeAmigos.Products.Services.UnderCut;

public class UnderCutService : IUnderCutService
{
    private readonly HttpClient _client;

    public UnderCutService(HttpClient client,
                                IConfiguration configuration)
    {
        var baseUrl = configuration["WebServices:UnderCut:BaseURL"] ?? "";
        client.BaseAddress = new System.Uri(baseUrl);
        client.Timeout = TimeSpan.FromSeconds(5);
        client.DefaultRequestHeaders.Add("Accept", "application/json");
        _client = client;
    }

    public async Task<IEnumerable<ProductDto>> GetProductsAsync()
    {
        var uri = "api/product";
        var response = await _client.GetAsync(uri);
        response.EnsureSuccessStatusCode();
        var products = await response.Content.ReadAsAsync<IEnumerable<ProductDto>>();
        return products;
    }
}