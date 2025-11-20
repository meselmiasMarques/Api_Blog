using System.Text.RegularExpressions;
using Blog.Data;
using Blog.Extensions;
using Blog.Models;
using Blog.Services;
using Blog.ViewModels;
using Blog.ViewModels.Accounts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using SecureIdentity.Password;

namespace Blog.Controllers;


[ApiController]
public class AccountController : ControllerBase
{
    [HttpPost("v1/accounts")]
    public async Task<IActionResult> PostAsync(
        [FromServices] ApplicationDbContext _context,
        [FromServices] EmailService emailService,
        [FromServices] TokenService _tokenService,
        [FromBody] RegisterViewModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<User>(ModelState.GetErrors()));

        var user = new User
        {
            Name = model.Name,
            Email = model.Email,
            
            Slug = model.Email.Replace("@", "-").Replace(".", "-")
        };

        var password = PasswordGenerator.Generate(25);
        user.PasswordHash = PasswordHasher.Hash(password);

        
        try
        {
            await _context.AddAsync(user);
            await _context.SaveChangesAsync();

            emailService.Send(
                user.Name,
                user.Email, 
                "Bem Vindo ao Blog! ",
                $"Sua senha é : <strong>{password}</strong>");
                
            
            return Ok(new ResultViewModel<dynamic>(new
            {
                user.Email,
                password
            }));
        }
        catch (DbUpdateException)
        {
            return StatusCode(400, "Este Email já está cadastrado");
        }
        catch 
        {
            return StatusCode(500, "erro interno no servidor");
        }
        
        
        return Ok();
    }

    [HttpPost("v1/accounts/login")]
    public async Task<IActionResult> Login(
        [FromServices] ApplicationDbContext _context,
        [FromServices] TokenService _tokenService,
        [FromBody] LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));
        
        var user = await _context.Users
            .AsNoTracking()
            .Include(r => r.Roles)
            .FirstOrDefaultAsync(u => u.Email == model.Email);
        
        if (user == null)
            return StatusCode(401 ,new ResultViewModel<string>("Usuário ou senha Inválidos"));

        if (!PasswordHasher.Verify(user.PasswordHash, model.Password))
            return StatusCode(401, new ResultViewModel<string>("Usuário ou senha Inválidos"));

        try
        {
            var token = _tokenService.GenerateToken(user);
            return Ok(token);
        }
        catch (Exception e)
        {
            return StatusCode(500, new ResultViewModel<string>("Erro interno no servidor"));
        }
    }

    [HttpPost("v1/accounts/upload-image")]
    public async Task<IActionResult> UploadImage(
        [FromBody] UploadImageViewModel model,
        [FromServices] ApplicationDbContext _context
        )
    {
        var fileName = $"{Guid.NewGuid().ToString()}.jpg";
        var data = new Regex(@"^data:image\/[a-z]+;base64,")
            .Replace(model.Base64Image, "");

        var bytes = Convert.FromBase64String(data);

        try
        {
            await System.IO.File.WriteAllBytesAsync($"wwwroot/images/{fileName}", bytes);
        }
        catch (Exception e)
        {
            return StatusCode(500, new ResultViewModel<string>("Erro interno no servidor"));
        }

        var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == User.Identity.Name);

        if (user == null)
        {
            return NotFound(new ResultViewModel<User>("Usuário não encontrado"));
        }

        user.Image = $"http://localhost:5116/images/{fileName}";

        try
        {
            _context.Update(user);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            return StatusCode(500, new ResultViewModel<string>("Erro interno no servidor"));

        }
        
        return Ok(new ResultViewModel<string>("Imagem alterada com  sucesso",null));

    }

}