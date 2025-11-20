using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Blog.Models;

public partial class User
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    [JsonIgnore]
    public string PasswordHash { get; set; } = null!;

    public string? Image { get; set; }

    public string Slug { get; set; } = null!;

    public string? Bio { get; set; }

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();

    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
}
