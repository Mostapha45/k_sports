using backend.Data;
using backend.Mappers;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controller
{

    /// <summary>
    /// Controller for handling product-related requests.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductsController"/> class.
    /// </summary>
    /// <param name="context">The data context.</param>
    public class ProductsController(DataContext context) : ControllerBase
    {

        /// <summary>
        /// Gets a list of products with optional filters and sorting.
        /// </summary>
        /// <param name="limit">The maximum number of products to return.</param>
        /// <param name="minPrice">The minimum price of the products to return.</param>
        /// <param name="sort">The sorting criteria for the products.</param>
        /// <param name="maxPrice">The maximum price of the products to return.</param>
        /// <param name="contains">A search term to filter products by name or description.</param>
        /// <param name="keywords">Additional keywords to filter products.</param>
        /// <returns>A list of products that match the specified criteria.</returns>
        [HttpGet]
        public ActionResult GetProducts(int? limit = null, double? minPrice = null, string? sort = null, double? maxPrice = null, string? contains = null, string? keywords = null)
        {

            try
            {

                IQueryable<Product> query = context.Products;

                if (!string.IsNullOrEmpty(contains))
                {
                    query = context.Products
                        .Where(p => EF.Functions
                            .ToTsVector("english", p.ItemName + " " + p.Description).Matches(EF.Functions.WebSearchToTsQuery("english", contains)))
                        .Select(p => new
                        {
                            Product = p,
                            Rank = EF.Functions
                                .ToTsVector("english", p.ItemName + " " + p.Description)
                                .Rank(EF.Functions.WebSearchToTsQuery("english", contains))
                        })
                        .OrderByDescending(p => p.Rank)
                        .Select(p => p.Product);
                }
                else
                {
                    query = context.Products;
                }

                // Apply filters based on query parameters
                if (minPrice != null)
                {
                    query = query.Where(p => p.Price >= minPrice);
                }

                if (maxPrice != null)
                {
                    query = query.Where(p => p.Price <= maxPrice);
                }

                if (!string.IsNullOrEmpty(keywords))
                {
                    var keywordsArray = keywords.Split(',')
                        .Select(w => w.Trim())
                        .Where(w => !string.IsNullOrEmpty(w)) // Filter out empty keywords
                        .ToList();

                    query = query.Where(p => keywordsArray.All(keyword =>
                            p.ProductRelatedWords.Any(prw =>
                                prw.Word == keyword)
                    ));
                }

                query = string.IsNullOrEmpty(sort) || sort.ToLower().Equals("price_asc")
                    ? query.OrderBy(p => p.Price)
                    : query.OrderByDescending(p => p.Price);

                query = query.Take(limit ?? 10);

                var products = query.Select(p => p.MapToProductResponseDto()).ToList();

                return Ok(products);
            }

            catch
            {
                throw;
            }
        }

    }

}
