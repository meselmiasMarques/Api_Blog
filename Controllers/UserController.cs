using Blog.Data;
using Blog.Models;
using Blog.ViewModels;
using Blog.ViewModels.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers;

[Authorize(Roles =  "admin")]
[ApiController]
public class UserController : ControllerBase
{
    [HttpGet("v1/users")]
    public async Task<IActionResult> GetAsync(
        [FromServices] ApplicationDbContext context,
        int page = 0,
        int pageSize = 25)
    {
        
        try
        {
            var count =await context.Users.AsNoTracking().CountAsync();
            var users = await context
                .Users
                .AsNoTracking()
                .Include(u => u.Roles)
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToListAsync();
            
            return Ok(new ResultViewModel<dynamic>(
                new
                {
                    total = count,
                    page,
                    pageSize,
                    users                }
                ));
        }
        catch
        {
            return BadRequest(new ResultViewModel<List<User>>("Erro interno no servidor"));
        }
    }
    
    [HttpGet("v1/users/{id:int}")]
    public async Task<IActionResult> GetAsync(
        [FromServices] ApplicationDbContext context,
        [FromRoute] int id)
    {
        
        try
        {
            var user = await context
                .Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
                return NotFound(new ResultViewModel<User>("usuario não  encontrado"));

            return Ok(new ResultViewModel<User>(user));
        }
        catch
        {
            return BadRequest(new ResultViewModel<User>("Erro interno no servidor"));
        }
    }
    
    [HttpDelete("v1/users/{id:int}")]
    public async Task<IActionResult> DeleteAsync(
        [FromServices] ApplicationDbContext context,
        [FromRoute] int id)
    {
        
        try
        {
            var user = await context
                .Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
                return NotFound(new ResultViewModel<User>("usuario não  encontrado"));
            
            context.Users.Remove(user);
            await context.SaveChangesAsync();
            
            return Ok(new ResultViewModel<User>(user));
        }
        catch
        {
            return BadRequest(new ResultViewModel<User>("Erro interno no servidor"));
        }
    }
    
    [HttpPut("v1/users/{id:int}")]
    public async Task<IActionResult> PutAsync(
        [FromServices] ApplicationDbContext context,
        [FromBody] EditorUserViewModel model,
        [FromRoute] int id)
    {
        
        try
        {
            var user = await context
                .Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
                return NotFound(new ResultViewModel<User>("usuario não  encontrado"));
            
            user.Name = model.Name;
            user.Email = model.Email;
            user.Bio = model.Bio;
            user.Image = model.Image;
            user.Slug = model.Slug;
            user.PasswordHash = model.PasswordHash;
            context.Users.Update(user);
            await context.SaveChangesAsync();
            
            return Ok(new ResultViewModel<User>(user));
        }
        catch
        {
            return BadRequest(new ResultViewModel<User>("Erro interno no servidor"));
        }
    }
    
}