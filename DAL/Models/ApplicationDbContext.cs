using Basic_Product_Catalog_Api.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace DAL.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> opt) : base(opt)
        {
        }

        public DbSet<Product> Products { get; set; }
    }
}
