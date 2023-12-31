using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using ThreeAmigos.WebApp.Services;

namespace ThreeAmigos.WebApp.Services{
    public class RefreshService : BackgroundService{
        private readonly ILogger<RefreshService> _logger;
        private readonly IProductService _productService;
        private const int RefreshIntervalInMinutes = 1;

        public RefreshService(IProductService productService, ILogger<RefreshService> logger){
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken){
            while (!stoppingToken.IsCancellationRequested){
                try{
                    _logger.LogInformation("Refreshing product data...");
                    await _productService.GetProductDataAsync();
                    _logger.LogInformation("Product data refresh complete.");
                    await Task.Delay(TimeSpan.FromMinutes(RefreshIntervalInMinutes), stoppingToken);
                }
                catch (Exception ex){
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }
    }
}
