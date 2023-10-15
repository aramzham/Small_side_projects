using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;

namespace FeatureFlags.Web.Controllers;

[Route("api/[controller]")]
[FeatureGate("NewControllerFlag")]
[ApiController]
public class ExampleController : ControllerBase
{
    public IActionResult Get()
    {
        return Ok("Hello from the new controller!");
    }
}