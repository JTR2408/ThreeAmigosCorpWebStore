// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Mvc.RazorPages;
// using System;
// using System.Net;
// using System.Security.Cryptography.X509Certificates;


// namespace ThreeAmigos.WebApp.Pages;

// public class IndexModel : PageModel
// {
//     private readonly ILogger<IndexModel> _logger;

//     public IndexModel(ILogger<IndexModel> logger)
//     {
//         _logger = logger;
//     }
 
//     public void OnGet()
//     {

//         Console.Write("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");


//         static async Task Main(String[] args){
//             var httpClient = HttpClientFactory.Create();

//             var url = "https://threeamigoscorpproducts.azurewebsites.net/debug/undercut";
//             HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(url);

//             if(httpResponseMessage.StatusCode == HttpStatusCode.OK){
//                 var content = httpResponseMessage.Content;
//                 var data = await content.ReadAsAsync<Data>();
//                 Console.WriteLine(data);
//                 if(data == null){
//                     Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
//                 }
//             }
//         }}
//            class Data{
//      public int Id {get; set;}

//     public string Name {get; set;} = string.Empty;
    
//     public float Price { get; set; }

//     public string BrandName { get; set; } = string.Empty;

//     public int BrandId { get; set; }

//     public string CategoryName { get; set; } = string.Empty;

//     public int CategoryId { get; set; }

//     public bool InStock { get; set; }

//         public override string ToString()
//         {
//             return $"{Id},{Name},{Price},{BrandName},{BrandId},{CategoryName},{CategoryId},{InStock}";
//         }

//     } 
//     }


using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System;
using ThreeAmigos.WebApp.Services;

namespace ThreeAmigos.WebApp.Pages
{
    public class IndexModel : PageModel
{
    private readonly IProductService _productService;

    public IndexModel(IProductService productService)
    {
        _productService = productService;
    }

    public List<ProductDto> Products { get; private set; }

    public async Task OnGetAsync()
    {
        try
        {
            // Fetch product data using ProductService
            Products = await _productService.GetProductDataAsync();
        }
        catch (Exception ex)
        {
            // Handle exception
        }
    }
}
}



    

