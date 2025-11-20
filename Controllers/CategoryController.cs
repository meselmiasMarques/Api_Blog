using Blog.Data;
using Blog.Extensions;
using Blog.Models;
using Blog.ViewModels;
using Blog.ViewModels.Categories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Blog.Controllers;

//[Authorize(Roles = "admin")]
[ApiController]
public class CategoryController : ControllerBase
{
    [HttpGet("v1/categories")]
    public async Task<IActionResult> GetAsync(
        [FromServices] IMemoryCache cache,
        [FromServices] ApplicationDbContext context)
    {
        try
        {
            var categories = await cache.GetOrCreateAsync(
                "CategoriesCache", entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);
                return GetCategories(context);
            });
            
            return Ok(new ResultViewModel<List<Category>>(categories));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<List<Category>>("Internal Server Error"));
        }
    }

    private async Task<List<Category>> GetCategories(ApplicationDbContext context)
    {
        var categories = await context
            .Categories
            .AsNoTracking()
            .ToListAsync();
        
        return categories;
    }

    [HttpGet("v1/categories/{id:int}")]
    public async Task<IActionResult> GetAsync(
        [FromServices] ApplicationDbContext context,
        [FromRoute] int id)
    {
        try
        {
            var category = await context
                .Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (category == null)
                return NotFound(new ResultViewModel<Category>($"Category with id {id} not found"));
            return Ok(new ResultViewModel<Category>(category));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<List<Category>>("Internal Server Error"));

        }
    }

    [HttpPost("v1/categories")]
    public async Task<IActionResult> PostAsync(
        [FromServices] ApplicationDbContext context,
        [FromBody] EditorCategoryViewModel model)
    {

        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<Category>(ModelState.GetErrors()));
        
        var category = new Category
        {
            Id = 0,
            Name = model.Name,
            Slug = model.Slug.ToLower()
        };
        
        try
        {
            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();
            return Created($"v1/categories/{category.Id}", new ResultViewModel<Category>(category));
        }
        catch 
        {
            return StatusCode(500, new ResultViewModel<List<Category>>("Internal Server Error"));
        }
    }

    [HttpPut("v1/categories/{id:int}")]
    public async Task<IActionResult> PutAsync(
        [FromServices] ApplicationDbContext context,
        [FromBody] EditorCategoryViewModel model, [FromRoute] int id)
    {
        
        try
        {
            var category = context.Categories.FirstOrDefault(x => x.Id == id);
            if (category == null)
                return NotFound(new ResultViewModel<Category>($"Category with id {id} not found"));
            
            category.Name = model.Name;
            category.Slug = model.Slug.ToLower();
            
            context.Categories.Update(category);
            await context.SaveChangesAsync();
            return Ok(new ResultViewModel<Category>(category));
        }
        catch 
        {
            return StatusCode(500, new ResultViewModel<List<Category>>("Internal Server Error"));
        }
    }
    
    [HttpDelete("v1/categories/{id:int}")]
    public async Task<IActionResult> Deleteasync(
        [FromServices] ApplicationDbContext context,
        [FromRoute] int id)
    {
        try
        {
            var category = context.Categories.FirstOrDefault(x => x.Id == id);
            if (category == null)
                return NotFound(new ResultViewModel<Category>($"Category with id {id} not found"));

            context.Categories.Remove(category);
            await context.SaveChangesAsync();
            return NoContent();
        }
        catch 
        {
            return StatusCode(500, new ResultViewModel<List<Category>>("Internal Server Error"));
        }
    }
}