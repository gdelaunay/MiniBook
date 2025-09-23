using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MiniBookApp.Models;

namespace MiniBookApp;

public class AppDbContext : IdentityDbContext<IdentityUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    public DbSet<Utilisateur> Utilisateurs => Set<Utilisateur>();
    public DbSet<Administrateur> Administrateurs => Set<Administrateur>();
    public DbSet<Post> Posts => Set<Post>();
    public DbSet<Commentaire> Commentaires => Set<Commentaire>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); 
        modelBuilder.Entity<Post>()
            .HasQueryFilter(p => EF.Property<string>(p, "Discriminator") == "Post");
    }

}