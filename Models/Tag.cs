using System;
using System.Collections.Generic;

namespace Blog.Models;

public partial class Tag
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Slug { get; set; } = null!;
}
