using backend.Data;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controller;


/// <summary>
/// Controller for seeding the database.
/// </summary>
[Route("api/[controller]")]
[ApiController]


/// <summary>
/// Initializes a new instance of the <see cref="SeedController"/> class.
/// </summary>
/// <param name="context">The data context.</param>
public class SeedController(DataContext context) : ControllerBase
{

    /// <summary>
    /// Seeds the database with initial data.
    /// </summary>
    /// <returns>An <see cref="ActionResult"/> indicating the result of the operation.</returns>
    [HttpPost]
    public ActionResult Seed()
    {
        try
        {
            DbSeeder.Seed(context);

            return Ok();
        }
        catch
        {
            throw;
        }
    }
}