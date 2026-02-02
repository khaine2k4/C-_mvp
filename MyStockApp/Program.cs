using Microsoft.EntityFrameworkCore;
using MyStockApp.Models;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/Login";
    });

builder.Services.AddDbContext<MyStockContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyStockDB")));

var app = builder.Build();

// Seed Data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<MyStockContext>();
    
    // Create DB if not exists
    context.Database.EnsureCreated();

    try
    {
        // Check if Users table exists and we can query it
        if (!context.Users.Any())
        {
            context.Users.AddRange(
                new MyStockApp.Models.User { Username = "admin", Password = "123", Role = "Admin" },
                new MyStockApp.Models.User { Username = "user", Password = "123", Role = "User" }
            );
            context.SaveChanges();
        }
    }
    catch (Exception ex)
    {
        // If error (likely "Invalid object name 'Users'"), recreate the DB
        // Note: This WILL DELETE existing data. Suitable for Dev/MVP only.
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogWarning(ex, "Database schema mismatch. Recreating database...");
        
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
        
        // Seed again after recreate
        if (!context.Users.Any())
        {
            context.Users.AddRange(
                new MyStockApp.Models.User { Username = "admin", Password = "123", Role = "Admin" },
                new MyStockApp.Models.User { Username = "user", Password = "123", Role = "User" }
            );
            context.SaveChanges();
        }
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
