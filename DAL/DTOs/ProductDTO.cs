using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTOs
{
    public class CreateProductDTO
    {
        [Required(ErrorMessage = "Product Name is required.")]
        [StringLength(100, ErrorMessage = "Product Name can't exceed 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        [StringLength(50, ErrorMessage = "Category can't exceed 50 characters.")]
        public string Category { get; set; }

        [StringLength(500, ErrorMessage = "Description can't exceed 500 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Stock Quantity is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Stock Quantity can't be negative.")]
        public int StockQuantity { get; set; }
    }

    public class UpdateProductDTO
    {
        [Required(ErrorMessage = "Product Name is required.")]
        [StringLength(100, ErrorMessage = "Product Name can't exceed 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        [StringLength(50, ErrorMessage = "Category can't exceed 50 characters.")]
        public string Category { get; set; }

        [StringLength(500, ErrorMessage = "Description can't exceed 500 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Stock Quantity is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Stock Quantity can't be negative.")]
        public int StockQuantity { get; set; }
    }
}
