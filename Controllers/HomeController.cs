using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniBookApp.Models;

namespace MiniBookApp.Controllers;

public class HomeController(ILogger<HomeController> logger, AppDbContext context) : Controller
{
    private readonly ILogger<HomeController> _logger = logger;
    private readonly AppDbContext _context = context;

    public IActionResult Index()
    {
        var posts = _context.Posts
                .Include(p => p.Auteur)
                .ToList();
        return View(posts);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}