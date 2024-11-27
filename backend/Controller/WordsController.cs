using backend.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controller
{

    /// <summary>
    /// Controller for handling word-related requests.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]

    /// <summary>
    /// Initializes a new instance of the <see cref="WordsController"/> class.
    /// </summary>
    /// <param name="context">The data context.</param>
    public class WordsController(DataContext context) : ControllerBase
    {

        /// <summary>
        /// Gets data related to words with optional filters.
        /// </summary>
        /// <param name="keywords">A comma-separated list of keywords to filter the words.</param>
        /// <param name="contains">A search term to filter products by name.</param>
        /// <returns>An <see cref="IActionResult"/> containing the filtered word data.</returns>
        
        [HttpGet]
        public IActionResult GetData(string? keywords, string? contains)
        {
            try
            {

                var query = context.ProductRelatedWords.AsQueryable();

                if (!string.IsNullOrWhiteSpace(contains))
                {
                    query = query.Where(prw =>
                        prw.Products.Any(p => EF.Functions.Like(p.ItemName, "%" + contains + "%")));
                }

                var keywordsHashSet =
                    new HashSet<string>(keywords?.Split(',').Select(w => w.Trim()) ?? Array.Empty<string>());

                var wordsWithCounts = query.Select(prw => new
                {
                    prw.Word,
                    ProductCount = keywordsHashSet.Count == 0
                            ? prw.Products.Count
                            : prw.Products.Count(p =>
                                keywordsHashSet.All(keyword =>
                                    p.ProductRelatedWords.Any(rw =>
                                        rw.Word == keyword))
                            )
                })
                    .Where(wc => wc.ProductCount > 0);

                return Ok(wordsWithCounts);
            }
            catch
            {
                throw;
            }
        }
    }
}