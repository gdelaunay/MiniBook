using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniBookApp.Models;

namespace MiniBookApp.Controllers;

public class HomeController(ILogger<HomeController> logger, AppDbContext context) : Controller
{
    private readonly ILogger<HomeController> _logger = logger;
    private readonly AppDbContext _context = context;
    
    public class ViewModel
    {
        public List<Post> Posts { get; set; } = [];
        public Post? NewPost { get; set; }
    }
    
    public IActionResult Index()
    {
        var vm = new ViewModel()
        {
            Posts = _context.Posts.OrderByDescending(p => p.DatePublication).Include(p => p.Auteur).ToList()
        };
        return View(vm);
    }
    /*
    [HttpPost]
    public async Task<IActionResult> Index(ViewModel vm)
    {
        if (vm.NewPost != null)
        {
            vm.NewPost.DatePublication = DateTime.Now;
            vm.NewPost.Auteur = _context.Utilisateurs.FirstOrDefault(u => u.Id == 1);
            _context.Posts.Add(vm.NewPost);
        }
        Console.WriteLine(vm.ToString());
        await _context.SaveChangesAsync();
        return Index();
    }

    /*
    [HttpPost]
    public async Task<IActionResult> Index(ViewModel vm)
    {
        if (vm.NewPost != null)
        {
            vm.NewPost.DatePublication = DateTime.Now;
            vm.NewPost.Auteur = _context.Utilisateurs.FirstOrDefault(u => u.Id == 1);
            _context.Posts.Add(vm.NewPost);
        }
        Console.WriteLine(vm.ToString());
        await _context.SaveChangesAsync();
        return Index();
    }*/

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