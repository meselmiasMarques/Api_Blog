using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels.Users;

public class EditorUserViewModel
{
    [Required(ErrorMessage = "o Nome é obrigatório")]
    [MaxLength(80,ErrorMessage = "Máximo 80 caracteres")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "o Email é obrigatório")]
    [MaxLength(80,ErrorMessage = "Máximo 80 caracteres")]
    [DataType(DataType.EmailAddress, ErrorMessage = "Formato email inválido")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "a senha é obrigatório")]
    [MaxLength(80,ErrorMessage = "Máximo 80 caracteres")]
    [DataType(DataType.Password)]
    public string PasswordHash { get; set; } = null!;
    
    [MaxLength(80,ErrorMessage = "Máximo 80 caracteres")]
    public string Bio { get; set; } = null!;

    
    [MaxLength(80,ErrorMessage = "Máximo 80 caracteres")]
    public string Image { get; set; } = null!;

    [Required(ErrorMessage = "o slug é obrigatório")]
    public string Slug { get; set; } = null!;
}