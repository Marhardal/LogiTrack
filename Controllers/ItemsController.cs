using LogiTrack;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class ItemsController : ControllerBase
{


    private readonly DBContext _context;

    public ItemsController(DBContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetItems()
    {
        var items = _context.InventoryItems.ToList();
        return Ok(items);
    }

    [HttpPost]
    public IActionResult AddItem(InventoryItem item)
    {
        _context.InventoryItems.Add(item);
        _context.SaveChanges();
        return Ok(item);
    }
}

