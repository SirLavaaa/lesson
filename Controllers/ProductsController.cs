using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using lesson.Data;
using lesson.Models;

namespace lesson.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _db;

    public ProductsController(AppDbContext db)
    {
        _db = db;
    }

    
     public async Task<ActionResult<IEnumerable<Product>>> GetProducts(
    [FromQuery] string? search,
    [FromQuery] string? category,
    [FromQuery] decimal? minPrice,
    [FromQuery] decimal? maxPrice,
    [FromQuery] bool? inStock,
    [FromQuery] string? sortBy,
    [FromQuery] string? sortDir
  )
  {
    IQueryable<Product> query = _db.Products;

    if (!string.IsNullOrWhiteSpace(search))
    {
      var searchLower = search.ToLower();

      query = query.Where(p => p.Name.ToLower().Contains(searchLower));
    }

    if (!string.IsNullOrWhiteSpace(category))
    {
      query = query.Where(p => p.Category.ToLower() == category.ToLower());
    }

    if (minPrice.HasValue)
    {
      query = query.Where(p => p.Price >= minPrice.Value);
    }

    if (maxPrice.HasValue)
    {
      query = query.Where(p => p.Price <= maxPrice.Value);
    }

    if (inStock.HasValue)
    {
      query = query.Where(p => p.InStock == inStock.Value);
    }

    var descending = sortDir?.ToLower() == "desc";

    query = sortBy?.ToLower() switch
    {
      "name" => descending ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name),
      "category" => descending ? query.OrderByDescending(p => p.Category) : query.OrderBy(p => p.Category),
      "price" => descending ? query.OrderByDescending(p => p.Price) : query.OrderBy(p => p.Price),
      _ => query
    };

    var products = await query.ToListAsync();
    return Ok(products);
  }
}