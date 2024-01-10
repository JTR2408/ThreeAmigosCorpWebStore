using System.Net.Http.Headers;
using Auth0.AspNetCore.Authentication;
using Newtonsoft.Json;
using ThreeAmigos.WebApp.Services;
using Newtonsoft.Json;


namespace ThreeAmigos.WebApp.Services;

    public class ProductService : IProductService{

    private readonly IHttpClientFactory _clientFactory;
    private readonly IConfiguration _configuration;

    public ProductService(IHttpClientFactory clientFactory, 
                            IConfiguration configuration){
        _clientFactory = clientFactory;
        _configuration = configuration;

    }
    record TokenDto(string access_token, string token_type, int expires_in);

    public async Task<List<ProductDto>> GetProductDataAsync(){
        var tokenClient = _clientFactory.CreateClient();

        var authBaseAddress = _configuration["AuthO:Authority"];
        tokenClient.BaseAddress = new Uri(authBaseAddress);

        var tokenParams = new Dictionary<string, string> {
            { "grant_type", "client_credentials" },
            { "client_id", _configuration["AuthO:ClientId"] },
            { "client_secret", _configuration["AuthO:ClientSecret"] },
            { "audience", _configuration["AuthO:Audience"] },
        };
        var tokenForm = new FormUrlEncodedContent(tokenParams);
        var tokenResponse = await tokenClient.PostAsync("/oauth/token", tokenForm);
        tokenResponse.EnsureSuccessStatusCode();
        var tokenInfo = await tokenResponse.Content.ReadFromJsonAsync<TokenDto>();


        var client = _clientFactory.CreateClient();

        var BaseURL = _configuration["Services:BaseURL"];
        client.BaseAddress = new Uri(BaseURL);
        client.DefaultRequestHeaders.Authorization = 
                    new AuthenticationHeaderValue("Bearer", tokenInfo?.access_token);
            try{
            var url = "https://threeamigoscorpproducts.azurewebsites.net/debug/undercut";

            var response = await client.GetAsync(url);
            
            if (response.IsSuccessStatusCode){
                string json = await response.Content.ReadAsStringAsync();
                List<ProductDto> products = JsonConvert.DeserializeObject<List<ProductDto>>(json);
                return products;
            }
            else{
                return new List<ProductDto>();
            }
        }
        catch (Exception ex){
            throw new Exception("Error fetching product data", ex);
        }
    }
    }
