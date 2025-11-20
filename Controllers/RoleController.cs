using Blog.Data;
using Blog.Extensions;
using Blog.Models;
using Blog.ViewModels;
using Blog.ViewModels.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers;

[Authorize(Roles = "admin")]
[ApiController]
public class RoleController : ControllerBase
{
    [HttpGet("v1/roles")]
    public async Task<IActionResult> GetAsync(
        [FromServices] ApplicationDbContext context)
    {
        try
        {
            var roles = await context.Roles
                .AsNoTracking()
                .ToListAsync();
            
            return Ok(new ResultViewModel<List<Role>>(roles));
        }
        catch
        {
            return BadRequest(new ResultViewModel<List<Role>>("Erro interno no servidor"));
        }
    }
    
    [HttpGet("v1/roles/{id:int}")]
    public async Task<IActionResult> GetAsync(
        [FromServices] ApplicationDbContext context,
        [FromRoute] int id)
    {
        try
        {
            var role = await context.Roles
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id);
            
            if (role == null)
                return NotFound(new ResultViewModel<Role>("Perfil não encontrado"));
            
            return Ok(new ResultViewModel<Role>(role));
            
        }
        catch
        {
            return BadRequest(new ResultViewModel<List<Role>>("Erro interno no servidor"));
        }
    }
    
    [HttpDelete("v1/roles/{id:int}")]
    public async Task<IActionResult> DeleteAsync(
        [FromServices] ApplicationDbContext context,
        [FromRoute] int id)
    {
        try
        {
            var role = await context.Roles
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id);
            
            if (role == null)
                return NotFound(new ResultViewModel<Role>("Perfil não encontrado"));
            
            context.Roles.Remove(role);
            await context.SaveChangesAsync();

            return NoContent();

        }
        catch
        {
            return BadRequest(new ResultViewModel<List<Role>>("Erro interno no servidor"));
        }
    }
    
    [HttpPut("v1/roles/{id:int}")]
    public async Task<IActionResult> PutAsync(
        [FromServices] ApplicationDbContext context,
        [FromBody] EditorRoleViewModel model,
        [FromRoute] int id)
    {
        try
        {
            var role = await context.Roles
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id);
            
            if (role == null)
                return NotFound(new ResultViewModel<Role>("Perfil não encontrado"));
            
            role.Name = model.Name;
            role.Slug = model.Slug;
            
            context.Roles.Update(role);
            await context.SaveChangesAsync();
            
            return Ok(new ResultViewModel<Role>(role));
            
        }
        catch
        {
            return BadRequest(new ResultViewModel<List<Role>>("Erro interno no servidor"));
        }
    }
    
    [HttpPost("v1/roles")]
    public async Task<IActionResult> PostAsync(
        [FromServices] ApplicationDbContext context,
        [FromBody] EditorRoleViewModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<Role>(ModelState.GetErrors()));

        var role = new Role
        {
            Id = 0,
            Name = model.Name,
            Slug = model.Slug
        };
        
        try
        {
            await context.Roles.AddAsync(role);
            await context.SaveChangesAsync();
            
            return Created($"v1/roles/{role.Id}", new ResultViewModel<Role>(role));
            
        }
        catch
        {
            return BadRequest(new ResultViewModel<List<Role>>("Erro interno no servidor"));
        }
    }
    
    [HttpPost("v1/roles/user")]
    public async Task<IActionResult> PostUserAsync(
        [FromServices] ApplicationDbContext context,
        [FromBody] EditorUserRoleViewModel model
        )
    {
        var user = await context.Users
            .Include(r => r.Roles)
            .FirstOrDefaultAsync(x => x.Id == model.UserId);
        
        if (user == null)
            return NotFound(new ResultViewModel<string>("Usuário  não encontrado"));

        var role = await context.Roles.FirstOrDefaultAsync(x => x.Id == model.RoleId);
        
        if (role == null)
            return NotFound(new ResultViewModel<string>("Perfil não encontrado"));

        if (user.Roles.Any(r => r.Id == role.Id))
            return BadRequest(new ResultViewModel<string>("O Usuário já possui esse perfil"));
        
        try
        {
            user.Roles.Add(role);
            await context.SaveChangesAsync();

            return Created($"v1/roles/{role.Id}", new ResultViewModel<string>("Perfil associado com sucesso"));
        }
        catch 
        {
            return StatusCode(500, new ResultViewModel<string>("Erro interno no servidor"));
        }
    }
    
    
    [HttpDelete("v1/roles/user/{userid:int}/{roleId:int}")]
    public async Task<IActionResult> DeleteUserAsync(
        [FromServices] ApplicationDbContext context,
        [FromRoute] int userid,
        [FromRoute] int roleId
    )
    {
        var user = await context.Users
            .Include(r => r.Roles)
            .FirstOrDefaultAsync(x => x.Id == userid);
        
        if (user == null)
            return NotFound(new ResultViewModel<string>("Usuário  não encontrado"));

        var role = await context.Roles.FirstOrDefaultAsync(x => x.Id == roleId);
        
        if (role == null)
            return NotFound(new ResultViewModel<string>("Perfil não encontrado"));
        
        try
        {
            user.Roles.Remove(role);
            await context.SaveChangesAsync();

            return Ok(new ResultViewModel<string>("Perfil desassociado com sucesso"));
        }
        catch 
        {
            return StatusCode(500, new ResultViewModel<string>("Erro interno no servidor"));
        }
    }

}