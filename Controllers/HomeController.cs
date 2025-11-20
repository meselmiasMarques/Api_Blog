using Blog.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers;

[ApiController]
[Route("")]
public class HomeController : ControllerBase
{
    //Headth Check
    [HttpGet("")]
    public IActionResult Get() => Ok("Heath check ok !");
}