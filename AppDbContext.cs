using Microsoft.EntityFrameworkCore;
using MiniBookApp.Models;

namespace MiniBookApp;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Utilisateur> Utilisateurs => Set<Utilisateur>();
    public DbSet<Administrateur> Administrateurs => Set<Administrateur>();
    public DbSet<Post> Posts => Set<Post>();
    public DbSet<Commentaire> Commentaires => Set<Commentaire>();

}