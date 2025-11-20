using Blog.Data;
using Blog.Extensions;
using Blog.Models;
using Blog.ViewModels;
using Blog.ViewModels.Tags;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers;

[Authorize(Roles = "admin, author")]
[ApiController]
public class TagController : ControllerBase
{
    [HttpGet("v1/tags")]
    public async Task<IActionResult> Get(
        [FromServices] ApplicationDbContext context, 
        int page = 0, 
        int pageSize = 25)
    {
        
        try
        {
            var count = await context.Tags.AsNoTracking().CountAsync();
            var tags = await context
                .Tags
                .AsNoTracking()
                .Skip(page)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new ResultViewModel<dynamic>(new
            {
                total = count,
                page,
                pageSize,
                tags
            }));
        }
        catch 
        {
            return BadRequest(new ResultViewModel<List<Tag>>("erro interno no servidor"));
        }
    }
    
    [HttpGet("v1/tags/{id:int}")]
    public async Task<IActionResult> GetAsync(
        [FromServices] ApplicationDbContext context, 
        [FromRoute] int id)
    {
        
        try
        {
            var tag = await context
                .Tags
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id);
            
            if (tag == null)
                return NotFound(new ResultViewModel<List<Tag>>("Tag não encontrada"));

            return Ok(new ResultViewModel<Tag>(tag));
        }
        catch 
        {
            return BadRequest(new ResultViewModel<List<Tag>>("erro interno no servidor"));
        }
    }


    [HttpPost("v1/tags")]
    public async Task<IActionResult> PostAsync(
        [FromServices] ApplicationDbContext context,
        [FromBody] EditorTagViewModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<Tag>(ModelState.GetErrors()));

        var tag = new Tag
        {
            Id = 0,
            Name = model.Name,
            Slug = model.Slug
        };

        try
        {
            await context.Tags.AddAsync(tag);
            await context.SaveChangesAsync();
            return Created($"v1/tags/{tag.Id}", new ResultViewModel<Tag>(tag));
        }
        catch
        {
            return BadRequest(new ResultViewModel<List<Tag>>("erro interno no servidor"));
        }
    }
    
    [HttpPut("v1/tags/{id:int}")]
    public async Task<IActionResult> PutAsync(
        [FromServices] ApplicationDbContext context,
        [FromBody] EditorTagViewModel model,
        [FromRoute] int id)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<Tag>(ModelState.GetErrors()));

        var tag = await context.Tags.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);
        if (tag == null)
            return BadRequest(new  ResultViewModel<Tag>("tag não encontrada"));
        
        tag.Name = model.Name;
        tag.Slug = model.Slug;

        try
        {
            context.Tags.Update(tag);
            await context.SaveChangesAsync();
            return Ok(new ResultViewModel<Tag>(tag));
        }
        catch
        {
            return BadRequest(new ResultViewModel<List<Tag>>("erro interno no servidor"));
        }
    }
    
    [HttpDelete("v1/tags/{id:int}")]
    public async Task<IActionResult> DeleteAsync(
        [FromServices] ApplicationDbContext context, 
        [FromRoute] int id)
    {
        
        try
        {
            var tag = await context
                .Tags
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id);
            
            if (tag == null)
                return NotFound(new ResultViewModel<List<Tag>>("Tag não encontrada"));
            
            context.Tags.Remove(tag);
            await context.SaveChangesAsync();

            return NoContent();
        }
        catch 
        {
            return BadRequest(new ResultViewModel<List<Tag>>("erro interno no servidor"));
        }
    }

}