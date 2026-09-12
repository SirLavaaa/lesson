using Microsoft.EntityFrameworkCore;
using lesson.Models;

namespace lesson.Data;

public class AppDbContext : DbContext
{
  public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
  {
  }

  public DbSet<Product> Products { get; set; }
}
