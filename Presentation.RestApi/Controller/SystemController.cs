using Application.Service;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.RestApi.Controller;

[Route("[controller]")]
[Controller]
public class SystemController(ContextSeeder contextSeeder) : ControllerBase
{
    [HttpGet("seed-context")]
    public async Task<ActionResult> SeedContext()
    {
        try
        {
            await contextSeeder.SeedData();
            return Ok("Context seeded successfully.");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}