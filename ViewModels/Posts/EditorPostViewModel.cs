using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels.Posts;

public class EditorPostViewModel
{
    [Required(ErrorMessage = "O Titulo é obrigatorio")]
    [MaxLength(160, ErrorMessage = "Você ultrapassou o limite e 160 caracteres")]
    public string Title { get; set; } = null!;

    [Required(ErrorMessage = "O resumo é obrigatorio")]
    [MaxLength(255, ErrorMessage = "Você ultrapassou o limite e 160 caracteres")]
    public string Summary { get; set; } = null!;

    [Required(ErrorMessage = "O corpo do post é obrigatorio")]
    public string Body { get; set; } = null!;

    
    [Required(ErrorMessage = "O slug é obrigatorio")]
    public string Slug { get; set; } = null!;
    
    [Required(ErrorMessage = "O Autor é obrigatorio")]
    public int AuthorId { get; set; }

    [Required(ErrorMessage = "A categoria é obrigatorio")]
    public int CategoryId { get; set; }
}