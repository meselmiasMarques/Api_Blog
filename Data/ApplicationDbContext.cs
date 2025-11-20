using System;
using System.Collections.Generic;
using Blog.Models;
using Microsoft.EntityFrameworkCore;

namespace Blog.Data;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<PostTag> PostTags { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost,1433;Database=Blog;User ID=sa;Password=1q2w3e4r@#$;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Category");

            entity.HasIndex(e => e.Slug, "IX_Category_Slug").IsUnique();

            entity.Property(e => e.Name).HasMaxLength(80);
            entity.Property(e => e.Slug)
                .HasMaxLength(80)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.ToTable("Post");

            entity.HasIndex(e => e.AuthorId, "IX_Post_AuthorId");

            entity.HasIndex(e => e.CategoryId, "IX_Post_CategoryId");

            entity.HasIndex(e => e.Slug, "IX_Post_Slug").IsUnique();

            entity.Property(e => e.LastUpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");

            entity.HasOne(d => d.Author).WithMany(p => p.Posts)
                .HasForeignKey(d => d.AuthorId)
                .HasConstraintName("FK_Post_Author");

            entity.HasOne(d => d.Category).WithMany(p => p.Posts)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_Post_Category");
        });

        modelBuilder.Entity<PostTag>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("PostTag");

            entity.HasIndex(e => e.TagId, "IX_PostTag_TagId");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Role");

            entity.HasMany(d => d.Users).WithMany(p => p.Roles)
                .UsingEntity<Dictionary<string, object>>(
                    "UserRole",
                    r => r.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK_UserRole_UserId"),
                    l => l.HasOne<Role>().WithMany()
                        .HasForeignKey("RoleId")
                        .HasConstraintName("FK_UserRole_RoleId"),
                    j =>
                    {
                        j.HasKey("RoleId", "UserId");
                        j.ToTable("UserRole");
                        j.HasIndex(new[] { "UserId" }, "IX_UserRole_UserId");
                    });
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.ToTable("Tag");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.HasIndex(e => e.Slug, "IX_User_Slug").IsUnique();

            entity.Property(e => e.Email)
                .HasMaxLength(160)
                .IsUnicode(false);
            entity.Property(e => e.Name).HasMaxLength(80);
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Slug)
                .HasMaxLength(80)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
