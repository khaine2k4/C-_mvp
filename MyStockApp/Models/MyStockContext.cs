using Microsoft.EntityFrameworkCore;
namespace MyStockApp.Models;

public class MyStockContext : DbContext
{
    public MyStockContext(DbContextOptions<MyStockContext> options) : base(options) { }
    public DbSet<Product> Products => Set<Product>();
    public DbSet<User> Users => Set<User>();
}
