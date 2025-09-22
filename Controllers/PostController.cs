using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniBookApp.Models;

namespace MiniBookApp.Controllers;

public class PostController(AppDbContext context) : Controller
{ 
    private readonly AppDbContext _context = context;

    [HttpGet]
    public IActionResult Get()
    {
        var posts = _context.Posts.OrderByDescending(p => p.DatePublication).Include(p => p.Auteur).ToList();
        return Json(posts);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Post post)
    {
        Console.WriteLine(post.Auteur);
        if (post != null)
        {
            post.DatePublication = DateTime.Now;
            post.Auteur = _context.Utilisateurs.FirstOrDefault(u => u.Id == 1);
            _context.Posts.Add(post);
        }
        await _context.SaveChangesAsync();
        return Created();
    }

}