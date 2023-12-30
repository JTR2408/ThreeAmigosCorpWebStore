using Newtonsoft.Json;
using ThreeAmigos.WebApp.Services;


namespace ThreeAmigos.WebApp.Services{
    public class ProductService : IProductService{
    private readonly HttpClient _httpClient;

    public ProductService(HttpClient httpClient){
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public async Task<List<ProductDto>> GetProductDataAsync(){
        try{
            var url = "https://threeamigoscorpproducts.azurewebsites.net/debug/undercut";

            HttpResponseMessage response = await _httpClient.GetAsync(url);

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
}
