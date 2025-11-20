using System;
using System.Collections.Generic;

namespace Blog.Models;

public partial class PostTag
{
    public int PostId { get; set; }

    public int TagId { get; set; }
}
