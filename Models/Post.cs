using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Blog.Models;

public partial class Post
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Summary { get; set; } = null!;

    public string Body { get; set; } = null!;

    public string Slug { get; set; } = null!;

    public DateTime CreateDate { get; set; }

    public DateTime LastUpdateDate { get; set; }

    public int CategoryId { get; set; }

    public int AuthorId { get; set; }


    public virtual User Author { get; set; } = null!;

    public virtual Category Category { get; set; } = null!;
}
