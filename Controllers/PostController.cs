using Blog.Data;
using Blog.Extensions;
using Blog.Models;
using Blog.ViewModels;
using Blog.ViewModels.Posts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers;

[Authorize(Roles = "admin, author")]
[ApiController]
public class PostController : ControllerBase
{
    [HttpGet("v1/posts")]
    public async Task<IActionResult> GetAsync(
        [FromServices] ApplicationDbContext context,
        [FromQuery] int page = 0,
        [FromQuery] int pageSize = 5
        )
    {
        try
        {
            var count = await context.Posts.AsNoTracking().CountAsync();
            var posts = await context
                .Posts
                .AsNoTracking()
                .Include(x => x.Author)
                .Include(x => x.Category)
                .Skip(page * pageSize)
                .Take(pageSize)
                .OrderByDescending(x => x.LastUpdateDate)
                .ToListAsync();

            if (posts == null)
                return NotFound(new ResultViewModel<List<Post>>("Posts Não encontrados"));

            return Ok(new ResultViewModel<dynamic>(new
            {
                total = count,
                page,
                pageSize,
                posts
            }));
        }
        catch
        {
            return BadRequest(new ResultViewModel<Post>("Erro interno do servidor"));
        }
    }

    [HttpGet("v1/posts/{id:int}")]
    public async Task<IActionResult> GetAsync(
        [FromServices] ApplicationDbContext context,
        [FromRoute] int id)
    {
        try
        {
            var post = await context
                .Posts
                .AsNoTracking()
                .Include(x => x.Author)
                    .ThenInclude(x => x.Roles)
                .Include(x => x.Category)
                .FirstOrDefaultAsync(x => x.Id == id);
            
            if (post == null)
                return NotFound(new ResultViewModel<Post>("Conteúdo Não encontrado"));

            return Ok(new ResultViewModel<Post>(post));
        }
        catch
        {
            return BadRequest(new ResultViewModel<Post>("Erro interno do servidor"));
        }
    }

    
    [HttpGet("v1/posts/category/{category}")]
    public async Task<IActionResult> GetbyCategoryAsync(
        [FromServices] ApplicationDbContext context,
        [FromRoute] string category,
        [FromQuery] int page = 0,
        [FromQuery] int pageSize = 25
    )
    {
        try
        {
            var count = await context.Posts.AsNoTracking().CountAsync();
            var posts = await context
                .Posts
                .AsNoTracking()
                .Include(x => x.Author)
                .Include(x => x.Category)
                .Where(x => x.Category.Slug == category)
                .Select(x => new ListPostViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Slug =  x.Slug,
                    LastUpdateDate = x.LastUpdateDate,
                    Category = x.Category.Name,
                    Author =  $"{x.Author.Name} ({x.Author.Email})"
                })
                .Skip(page * pageSize)
                .Take(pageSize)
                .OrderByDescending(x => x.LastUpdateDate)
                .ToListAsync();

            if (posts == null)
                return NotFound(new ResultViewModel<List<Post>>("Posts Não encontrados"));

            return Ok(new ResultViewModel<dynamic>(new
            {
                total = count,
                page,
                pageSize,
                posts
            }));
        }
        catch
        {
            return BadRequest(new ResultViewModel<Post>("Erro interno do servidor"));
        }
    }
    
    
    
    [HttpPut("v1/posts/{id:int}")]
    public async Task<IActionResult> PutAsync(
        [FromServices] ApplicationDbContext context,
        [FromRoute] int id,
        [FromBody] EditorPostViewModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<Post>(ModelState.GetErrors()));
        try
        {
            var post = await context.Posts.FirstOrDefaultAsync(x => x.Id == id);

            if (post == null)
                return NotFound(new ResultViewModel<Post>("Post Não encontrado"));

            post.Title = model.Title;
            post.Summary = model.Summary;
            post.Body = model.Body;
            
            context.Posts.Update(post);
            await context.SaveChangesAsync();

            return Ok(new ResultViewModel<Post>(post));
        }
        catch
        {
            return BadRequest(new ResultViewModel<Post>("Erro inesperado ao atualizar post"));
        }
    }

    [HttpPost("v1/posts")]
    public async Task<IActionResult> PostAsync(
        [FromServices] ApplicationDbContext context,
        [FromBody] EditorPostViewModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<Post>(ModelState.GetErrors()));

        var post = new Post
        {
            Id = 0,
            Title = model.Title,
            Summary = model.Summary,
            Body = model.Body,
            Slug = model.Slug,
            CreateDate = DateTime.UtcNow,
            LastUpdateDate = DateTime.UtcNow
            // AuthorId = context.Users.AsNoTracking().FirstOrDefault(x => x.Email == User.Identity.Name).Id,
            // CategoryId = context.Categories.FirstOrDefault(x => x.Id == model.CategoryId)
            //
        };

        try
        {
            await context.Posts.AddAsync(post);
            await context.SaveChangesAsync();

            return Ok(new ResultViewModel<Post>(post));
        }
        catch (DbUpdateException ex)
        {
            return BadRequest(new ResultViewModel<Post>($"{ex.Message}"));
        }
        catch
        {
            return BadRequest(new ResultViewModel<Post>("Erro inesperado ao Cadastrar post"));
        }
    }

    [HttpDelete("v1/posts/{id:int}")]
    public async Task<IActionResult> DeleteAsync(
        [FromServices] ApplicationDbContext context,
        [FromRoute] int id)
    {
        try
        {
            var post = await context.Posts.FirstOrDefaultAsync(x => x.Id == id);

            if (post == null)
                return NotFound(new ResultViewModel<Post>("Post Não encontrado"));

            context.Posts.Remove(post);
            await context.SaveChangesAsync();

            return NoContent();
        }
        catch
        {
            return BadRequest(new ResultViewModel<Post>("Erro inesperado ao Excluír post"));
        }
    }
}