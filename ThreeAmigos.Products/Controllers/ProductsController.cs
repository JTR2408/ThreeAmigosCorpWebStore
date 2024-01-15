using Microsoft.AspNetCore.Mvc;
using ThreeAmigos.Products.Services.ProductRepo;
using ThreeAmigos.Products.Services.UnderCut;

namespace ThreeAmigos.Products.Controllers;

[ApiController]
[Route ("[controller]")]
public class ProductsController : ControllerBase{

 private readonly ILogger _logger;
    private readonly IUnderCutService _underCutService;
    private readonly IProductsRepo _productsRepo;

    public ProductsController(ILogger<ProductsController> logger,
                             IUnderCutService underCutService,
                             IProductsRepo productsRepo)
    {
        _logger = logger;
        _underCutService = underCutService;
        _productsRepo = productsRepo;
    }

    // /products/undercut
    [HttpGet("UnderCut")]
    public async Task<IActionResult> UnderCut(){
        IEnumerable<ProductDto> products = null!;
        try{
            products = await _underCutService.GetProductsAsync();
        }
        catch{
            _logger.LogWarning("An Error has Occurred With UnderCut Service");
            products = Array.Empty<ProductDto>();
        }
        return Ok(products.ToList());
    }

    // /products/repo
    [HttpGet("repo")]
    public async Task<IActionResult> Repo(){
        IEnumerable<Product> products = null;
        try{
            products = await _productsRepo.GetProductsAsync();
        }
        catch{
            _logger.LogWarning("An Error has Occurred With Products Service");
            products = Array.Empty<Product>();
        }
        return Ok(products.ToList());
    }
}