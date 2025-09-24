using Microsoft.AspNetCore.Authorization;
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
    
    [HttpGet("{id}")]
    //[Authorize]
    public async Task<IActionResult> GetById(int id)
    {
        var post = await _context.Posts
            .Include(p => p.Auteur)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (post == null)
            return NotFound();

        return Json(post);
    }

    [HttpPost]
    //[Authorize]
    //[ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Post post)
    {
        // In html form →   @Html.AntiForgeryToken()
        Console.WriteLine(post.Auteur);
        if (post != null)
        {
            post.DatePublication = DateTime.Now;
            post.Auteur = _context.Utilisateurs.FirstOrDefault();
            _context.Posts.Add(post);
        }
        await _context.SaveChangesAsync();
        return Created();
    }
    
    [HttpDelete]
    //[Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int postId)
    {
        var post = await _context.Posts.FindAsync(postId);
        if (post == null)
        {
            return NotFound();
        }
        _context.Posts.Remove(post);
        await _context.SaveChangesAsync();
        return NoContent();
    }

}