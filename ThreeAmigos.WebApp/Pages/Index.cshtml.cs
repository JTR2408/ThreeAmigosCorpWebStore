using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System;
using ThreeAmigos.WebApp.Services;

namespace ThreeAmigos.WebApp.Pages{
    public class IndexModel : PageModel{
    private readonly IProductService _productService;

    public IndexModel(IProductService productService){
        _productService = productService;
    }

    public List<ProductDto> Products { get; private set; }

    public async Task OnGetAsync(string searchTerm){
            try{
                if (!string.IsNullOrWhiteSpace(searchTerm)){
                    var allProducts = await _productService.GetProductDataAsync();
                    Products = allProducts.Where(p =>
                        p.Name.ToLower().Contains(searchTerm) ||
                        p.Description.ToLower().Contains(searchTerm) ||
                        p.BrandName.ToLower().Contains(searchTerm) ||
                        p.CategoryName.ToLower().Contains(searchTerm)
                    ).ToList();
                }
                else{
                    Products = await _productService.GetProductDataAsync();
                }
            }
            catch (Exception ex){
            }

    }
}
}



    

