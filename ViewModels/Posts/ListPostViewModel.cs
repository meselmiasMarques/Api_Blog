namespace Blog.ViewModels.Posts;

public class ListPostViewModel
{
    public int Id { get; set; }
    
    public string Title { get; set; } = null!;
    
    public string Slug { get; set; } = null!;
    
    public DateTime LastUpdateDate { get; set; }

    public string Author { get; set; } 
    
    public string Category { get; set; } 

}