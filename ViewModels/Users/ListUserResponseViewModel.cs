namespace Blog.ViewModels.Users;

public class ListUserResponseViewModel
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;
    
    public string Bio { get; set; } = null!;

    public string Image { get; set; } = null!;

    public string Slug { get; set; } = null!;
    
    

}