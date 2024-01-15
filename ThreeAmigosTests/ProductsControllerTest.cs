using System.Net.WebSockets;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ThreeAmigos.Products.Controllers;
using ThreeAmigos.Products.Services.UnderCut;

namespace ThreeAmigosTests;

[TestClass]
public class ProdctsControllerTests
{
        private ProductsController _productsController;
        private Mock<ILogger<ProductsController>> _loggerMock;
        private Mock<IUnderCutService> _underCutServiceMock;

        [TestInitialize]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<ProductsController>>();
            _underCutServiceMock = new Mock<IUnderCutService>();
            _productsController = new ProductsController(_loggerMock.Object, _underCutServiceMock.Object);
        }

        [TestMethod]
        public async Task UnderCutTest()
        {
            var expectedProducts = new List<ProductDto>();
            _underCutServiceMock.Setup(c => c.GetProductsAsync())
            .Returns(Task.FromResult<IEnumerable<ProductDto>>(expectedProducts));

            var result = await _productsController.UnderCut();

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = (OkObjectResult)result;
            var actualProducts = okResult.Value as List<ProductDto>;
            CollectionAssert.AreEqual(expectedProducts, actualProducts);
        }
}