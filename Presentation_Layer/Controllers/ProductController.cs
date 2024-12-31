using BAL;
using Basic_Product_Catalog_Api.Models.Entities;
using DAL.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation_Layer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: api/GetAllProducts
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var products = await _productService.GetAllProductsAsync();
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving products.", details = ex.Message });
            }
        }

        // GET: api/GetProductById/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(id);
                if (product == null)
                {
                    return NotFound(new { message = $"Product with ID {id} not found." });
                }
                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the product.", details = ex.Message });
            }
        }

        // POST: api/AddProduct
        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] CreateProductDTO createProductDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Invalid product data.", errors = ModelState });
            }

            try
            {
                // Map CreateProductDTO to Product entity
                var product = new Product
                {
                    Name = createProductDTO.Name,
                    Category = createProductDTO.Category,
                    Description = createProductDTO.Description,
                    Price = createProductDTO.Price,
                    StockQuantity = createProductDTO.StockQuantity
                };

                await _productService.AddProductAsync(product);
                return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while adding the product.", details = ex.Message });
            }
        }



        // PUT: api/Updateproducts/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductDTO updateProductDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Invalid product data.", errors = ModelState });
            }

            try
            {
                var existingProduct = await _productService.GetProductByIdAsync(id);
                if (existingProduct == null)
                {
                    return NotFound(new { message = $"Product with ID {id} not found." });
                }

                // Update properties of the existing product using UpdateProductDTO
                existingProduct.Name = updateProductDTO.Name;
                existingProduct.Category = updateProductDTO.Category;
                existingProduct.Description = updateProductDTO.Description;
                existingProduct.Price = updateProductDTO.Price;
                existingProduct.StockQuantity = updateProductDTO.StockQuantity;

                await _productService.UpdateProductAsync(existingProduct);
                return Ok("Updated Successfully");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the product.", details = ex.Message });
            }
        }


        // DELETE: api/Deleteproducts/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(id);
                if (product == null)
                {
                    return NotFound(new { message = $"Product with ID {id} not found." });
                }

                await _productService.DeleteProductAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the product.", details = ex.Message });
            }
        }
    }
}
