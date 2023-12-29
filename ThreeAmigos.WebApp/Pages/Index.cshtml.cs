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



    

