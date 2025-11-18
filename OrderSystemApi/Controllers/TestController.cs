
using Microsoft.AspNetCore.Mvc;

namespace OrderSystemApi.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    [HttpGet]
    public object Get() => new { message="API rodando!" };
}
