using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniBookApp;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Ajout d'identity
builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<IdentityUser>().AddRoles<IdentityRole>().AddEntityFrameworkStores<AppDbContext>();
// Utilisation BCRYPT
builder.Services.AddScoped<IPasswordHasher<IdentityUser>, BCryptPasswordHasher<IdentityUser>>();

// Ajout du service BDD
builder.Services.AddDbContext<AppDbContext>(options => options.UseMySQL(Environment.GetEnvironmentVariable("MYSQLCONNSTR_localdb")!));
    
    //"Server=localhost;Port=3306;Database=MiniBook;User=myuserminibook;Password=mypasswordminibook;"));

    // Ajout d'une configuration Cors
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
        policy =>
        {
            policy.WithOrigins(allowedOrigins!) //"https://minibook.gdelaunay.fr" 
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                .WithExposedHeaders("Location");
        });
});

// Configuration cookie same-site : protection CRSF
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.SameSite = SameSiteMode.Strict;
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowSpecificOrigins");
// Use identity
app.UseAuthorization();
app.MapIdentityApi<IdentityUser>();

// Désactive le script externe et inline : protection XSS
app.Use(async (context, next) =>
{
    context.Response.Headers.Append("Content-Security-Policy", "default-src 'self'; script-src 'self'");
    await next();
});

app.MapStaticAssets();


// création des roles et d'un admin
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

    string[] roles = ["Admin", "User"];
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }


    var admin = await userManager.FindByEmailAsync("admin@example.com");
    if (admin == null)
    {
        admin = new IdentityUser { UserName = "admin@example.com", Email = "admin@example.com" };
        await userManager.CreateAsync(admin, "Test1234!");
        await userManager.AddToRoleAsync(admin, "Admin");
    }
}

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();




// ------------------------------ CONFIGURATION ------------------------------ //

var identityGroup = app.MapGroup("/api");
identityGroup.MapIdentityApi<IdentityUser>();

// Mappage d'une route de déconnexion, pas incluse dans l'IdentityApi pour une certaine raison
identityGroup.MapPost("/logout", async (SignInManager<IdentityUser> signInManager, [FromBody] object empty) =>
    {
        if (empty != null)
        {
            await signInManager.SignOutAsync();
            return Results.Ok();
        }
        return Results.Unauthorized();
    })
    .RequireAuthorization();

// Mappage de suppression de compte, idem
identityGroup.MapDelete("/account", async (UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, HttpContext http) =>
    {
        var user = await userManager.GetUserAsync(http.User);
        if (user == null) return Results.BadRequest("Utilisateur non trouvé.");

        var result = await userManager.DeleteAsync(user);
        if (!result.Succeeded) return Results.BadRequest(result.Errors);

        await signInManager.SignOutAsync();
        return Results.NoContent();
    })
    .RequireAuthorization();

// Mappage récupération du compte connecté
identityGroup.MapGet("/account", async (UserManager<IdentityUser> userManager, HttpContext http) =>
    {
        var user = await userManager.GetUserAsync(http.User);
        if (user == null) return Results.Unauthorized();

        return Results.Ok(new {
            Id = user.Id,
            UserName = user.UserName
        });
    })
    .RequireAuthorization();




app.Run();

// Hasher BCRYPT
public class BCryptPasswordHasher<TUser> : Microsoft.AspNetCore.Identity.IPasswordHasher<TUser> where TUser : class
{
    public string HashPassword(TUser user, string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public PasswordVerificationResult VerifyHashedPassword(TUser user, string hashedPassword, string providedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword) 
            ? PasswordVerificationResult.Success 
            : PasswordVerificationResult.Failed;
    }
}