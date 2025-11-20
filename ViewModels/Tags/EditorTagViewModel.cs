using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels.Tags;

public class EditorTagViewModel
{
    [Required(ErrorMessage = "O nome é obrigatorio")]
    [MaxLength(160, ErrorMessage = "Máximo de 160 caracteres")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "O slug é obrigatorio")]
    [MaxLength(160, ErrorMessage = "Máximo de 160 caracteres")]
    public string Slug { get; set; } = null!;
}