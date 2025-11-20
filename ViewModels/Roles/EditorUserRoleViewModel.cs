using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels.Roles;

public class EditorUserRoleViewModel
{
    [Required(ErrorMessage = "O Usuário é obrigatorio")]
    public int UserId { get; set; }
    
    [Required(ErrorMessage = "O Perfil é obrigatorio")]
    public int RoleId { get; set; }
    
}