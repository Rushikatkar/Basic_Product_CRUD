using Basic_Product_Catalog_Api.Models.Entities;
using DAL.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BAL.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _productRepository.GetAllProductsAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _productRepository.GetProductByIdAsync(id);
        }

        public async Task AddProductAsync(Product product)
        {
            // Example of adding business logic: ensure price is positive
            if (product.Price <= 0)
            {
                throw new System.ArgumentException("Price must be greater than zero.");
            }

            await _productRepository.AddProductAsync(product);
        }

        public async Task UpdateProductAsync(Product product)
        {
            // Example of adding business logic: validate stock quantity
            if (product.StockQuantity < 0)
            {
                throw new System.ArgumentException("Stock Quantity cannot be negative.");
            }

            await _productRepository.UpdateProductAsync(product);
        }

        public async Task DeleteProductAsync(int id)
        {
            await _productRepository.DeleteProductAsync(id);
        }
    }
}
