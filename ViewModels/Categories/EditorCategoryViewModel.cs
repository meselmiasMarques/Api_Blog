using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels.Categories;

public class EditorCategoryViewModel
{
    [Required(ErrorMessage = "Nome é obrigatório")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "o slug é obrigatório")]
    public string Slug { get; set; } = null!;
}