using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Presentation_Layer.Controllers;
using Basic_Product_Catalog_Api.Models.Entities;
using DAL.DTOs;
using BAL;
using System.Threading.Tasks;
using System;

namespace Presentation_Layer.Controllers
{
    [TestFixture]
    public class ProductControllerTests
    {
        private Mock<IProductService> _mockProductService;
        private ProductController _controller;

        [SetUp]
        public void SetUp()
        {
            _mockProductService = new Mock<IProductService>();
            _controller = new ProductController(_mockProductService.Object);
        }

        #region **GetAllProducts Tests**
        [Test]
        public async Task GetAllProducts_ReturnsOkResult_WhenProductsExist()
        {
            // Arrange
            var products = new[] { new Product { Id = 1, Name = "Product1" } };
            _mockProductService.Setup(service => service.GetAllProductsAsync()).ReturnsAsync(products);

            // Act
            var result = await _controller.GetAllProducts();

            // Assert
            Assert.That(result, Is.TypeOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.EqualTo(products));
        }

        [Test]
        public async Task GetAllProducts_ReturnsServerError_WhenExceptionOccurs()
        {
            // Arrange
            _mockProductService.Setup(service => service.GetAllProductsAsync()).ThrowsAsync(new Exception("Server error"));

            // Act
            var result = await _controller.GetAllProducts();

            // Assert
            Assert.That(result, Is.TypeOf<ObjectResult>());
            var objectResult = result as ObjectResult;
            Assert.That(objectResult.StatusCode, Is.EqualTo(500));

            var errorResponse = objectResult.Value as dynamic;
            Assert.That(errorResponse, Is.Not.Null);
            Assert.That(errorResponse.message, Is.EqualTo("An error occurred while retrieving products."));
            Assert.That(errorResponse.details, Is.EqualTo("Server error"));
        }

        #endregion

        #region **GetProductById Tests**
        [Test]
        public async Task GetProductById_ReturnsOkResult_WhenProductExists()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Product1" };
            _mockProductService.Setup(service => service.GetProductByIdAsync(1)).ReturnsAsync(product);

            // Act
            var result = await _controller.GetProductById(1);

            // Assert
            Assert.That(result, Is.TypeOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.EqualTo(product));
        }

        [Test]
        public async Task GetProductById_ReturnsNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            _mockProductService.Setup(service => service.GetProductByIdAsync(1)).ReturnsAsync((Product)null);

            // Act
            var result = await _controller.GetProductById(1);

            // Assert
            Assert.That(result, Is.TypeOf<NotFoundObjectResult>());
            var notFoundResult = result as NotFoundObjectResult;
            Assert.That(notFoundResult.StatusCode, Is.EqualTo(404));

            var errorResponse = notFoundResult.Value as dynamic;
            Assert.That(errorResponse, Is.Not.Null);
            Assert.That(errorResponse.message, Is.EqualTo("Product with ID 1 not found."));
        }

        [Test]
        public async Task GetProductById_ReturnsServerError_WhenExceptionOccurs()
        {
            // Arrange
            _mockProductService.Setup(service => service.GetProductByIdAsync(1)).ThrowsAsync(new Exception("Server error"));

            // Act
            var result = await _controller.GetProductById(1);

            // Assert
            Assert.That(result, Is.TypeOf<ObjectResult>());
            var objectResult = result as ObjectResult;
            Assert.That(objectResult.StatusCode, Is.EqualTo(500));

            var response = objectResult.Value as dynamic;
            Assert.That(response.message, Is.EqualTo("An error occurred while retrieving the product."));
            Assert.That(response.details, Is.EqualTo("Server error"));
        }


        #endregion

        #region **AddProduct Tests**
        [Test]
        public async Task AddProduct_ReturnsCreatedAtAction_WhenProductIsValid()
        {
            // Arrange
            var createProductDTO = new CreateProductDTO
            {
                Name = "New Product",
                Category = "Electronics",
                Description = "A test product",
                Price = 100,
                StockQuantity = 10
            };

            // Create the expected Product that should be returned after creation
            var createdProduct = new Product
            {
                Id = 1, // Simulate identity-generated ID
                Name = createProductDTO.Name,
                Category = createProductDTO.Category,
                Price = createProductDTO.Price,
                StockQuantity = createProductDTO.StockQuantity
            };

            // Mock the AddProductAsync method to simulate a product being created
            _mockProductService.Setup(service => service.AddProductAsync(It.IsAny<Product>()))
                .Callback<Product>(product => product.Id = 1)  // Set the Id on the product passed to the method
                .Returns(Task.CompletedTask);

            // Mock the GetProductByIdAsync to return the created product
            _mockProductService.Setup(service => service.GetProductByIdAsync(1)).ReturnsAsync(createdProduct);

            // Act
            var result = await _controller.AddProduct(createProductDTO);

            // Assert
            Assert.That(result, Is.TypeOf<CreatedAtActionResult>());
            var createdResult = result as CreatedAtActionResult;
            Assert.That(createdResult.StatusCode, Is.EqualTo(201));

            // Assert that the returned product matches the expected created product
            var returnedProduct = createdResult.Value as Product;
            Assert.That(returnedProduct, Is.Not.Null);
            Assert.That(returnedProduct.Id, Is.EqualTo(1));  // Ensure the Id is set correctly
            Assert.That(returnedProduct.Name, Is.EqualTo(createdProduct.Name));
            Assert.That(returnedProduct.Category, Is.EqualTo(createdProduct.Category));
            Assert.That(returnedProduct.Price, Is.EqualTo(createdProduct.Price));
            Assert.That(returnedProduct.StockQuantity, Is.EqualTo(createdProduct.StockQuantity));
        }


        [Test]
        public async Task AddProduct_ReturnsBadRequest_WhenModelIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Name", "Name is required.");
            var createProductDTO = new CreateProductDTO();

            // Act
            var result = await _controller.AddProduct(createProductDTO);

            // Assert
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult.StatusCode, Is.EqualTo(400));
            Assert.That(badRequestResult.Value, Is.Not.Null);
        }

        [Test]
        public async Task AddProduct_ReturnsServerError_WhenExceptionOccurs()
        {
            // Arrange
            var createProductDTO = new CreateProductDTO { Name = "New Product", Price = 100 };
            _mockProductService.Setup(service => service.AddProductAsync(It.IsAny<Product>())).ThrowsAsync(new Exception("Server error"));

            // Act
            var result = await _controller.AddProduct(createProductDTO);

            // Assert
            Assert.That(result, Is.TypeOf<ObjectResult>());
            var objectResult = result as ObjectResult;
            Assert.That(objectResult.StatusCode, Is.EqualTo(500));

            var errorResponse = objectResult.Value as dynamic;
            Assert.That(errorResponse, Is.Not.Null);
            Assert.That(errorResponse.message, Is.EqualTo("An error occurred while adding the product."));
        }
        #endregion

        #region **UpdateProduct Tests**
        [Test]
        public async Task UpdateProduct_ReturnsOkResult_WhenProductIsUpdated()
        {
            // Arrange
            var updateProductDTO = new UpdateProductDTO
            {
                Name = "Updated Product",
                Category = "Electronics",
                Price = 120,
                StockQuantity = 15
            };
            var existingProduct = new Product { Id = 1, Name = "Old Product", Price = 100, StockQuantity = 10 };
            _mockProductService.Setup(service => service.GetProductByIdAsync(1)).ReturnsAsync(existingProduct);
            _mockProductService.Setup(service => service.UpdateProductAsync(It.IsAny<Product>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateProduct(1, updateProductDTO);

            // Assert
            Assert.That(result, Is.TypeOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.EqualTo("Updated Successfully"));
        }

        [Test]
        public async Task UpdateProduct_ReturnsNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            var updateProductDTO = new UpdateProductDTO { Name = "Updated Product", Price = 120 };
            _mockProductService.Setup(service => service.GetProductByIdAsync(1)).ReturnsAsync((Product)null);

            // Act
            var result = await _controller.UpdateProduct(1, updateProductDTO);

            // Assert
            Assert.That(result, Is.TypeOf<NotFoundObjectResult>());
            var notFoundResult = result as NotFoundObjectResult;
            Assert.That(notFoundResult.StatusCode, Is.EqualTo(404));

            var errorResponse = notFoundResult.Value as dynamic;
            Assert.That(errorResponse, Is.Not.Null);
            Assert.That(errorResponse.message, Is.EqualTo("Product with ID 1 not found."));
        }
        #endregion
    }
}
