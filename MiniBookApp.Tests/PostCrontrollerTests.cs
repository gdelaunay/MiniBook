using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniBookApp.Controllers;
using MiniBookApp.Models;
using Xunit;

namespace MiniBookApp.MiniBookApp.Tests;

public class PostControllerTests
{
    
    private static AppDbContext GetInMemoryDb()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        return new AppDbContext(options);
    }
    
    [Fact]
    public async Task CreatePost_ReturnsCreated()
    {
        await using var context = GetInMemoryDb();
        var auteur = new Utilisateur
            { Id = 1, Nom = "Delaunay", Prénom = "Goulwen", DateNaissance = DateTime.Parse("21/04/1998"), Email = "email@gd.com" };
        context.Utilisateurs.Add(auteur);
        await context.SaveChangesAsync();
        
        var postController = new PostController(context);
        var result = await postController.Create(new Post{ Auteur = auteur, DatePublication = new DateTime(), Contenu = "Contenu du post"});
        
        Assert.IsType<CreatedResult>(result);
    }
}