using Microsoft.AspNetCore.Mvc;

namespace Employee.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
    }
}
