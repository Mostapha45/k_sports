using Microsoft.EntityFrameworkCore;
using backend.Models;

namespace backend.Data
{

    /// <summary>
    /// Represents the data context for the application.
    /// </summary>
    public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
    {

        /// <summary>
        /// Gets or sets the Products DbSet.
        /// </summary>
        public DbSet<Product> Products { get; set; }

        /// <summary>
        /// Gets or sets the ProductRelatedWords DbSet.
        /// </summary>
        public DbSet<ProductRelatedWord> ProductRelatedWords { get; set; }

        /// <summary>
        /// Configures the model that was discovered by convention from the entity types
        /// exposed in <see cref="DbSet{TEntity}"/> properties on the context.
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for this context.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasIndex(p => new { p.ItemName, p.Description }).HasMethod("GIN")
                .IsTsVectorExpressionIndex("english");
        }
    }
}