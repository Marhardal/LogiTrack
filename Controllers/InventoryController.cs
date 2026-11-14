using LogiTrack;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
[Authorize]
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
        var items = _context.InventoryItems.AsNoTracking().Select(
            x => new InventoryItem
            {
                ID = x.ID,
                Name = x.Name,
                Quantity = x.Quantity,
                Location = x.Location,
                OrderID = x.OrderID,
                Order = x.Order
            }
        ).ToList();
        return Ok(items);
    }

    [HttpGet("{id}")]
    public IActionResult GetItem(int id)
    {
        var item = _context.InventoryItems.FirstOrDefault(x => x.ID == id);
        if (item == null)
        {
            return NotFound();
        }
        return Ok(item);
    }

    [HttpPost]
    [Authorize(Roles = "Manager")]
    public IActionResult AddItem(InventoryItem item)
    {
        _context.InventoryItems.Add(item);
        _context.SaveChanges();
        return Ok(item);
    }

    [HttpDelete("{id}")]
    public IActionResult RemoveItem(int id)
    {
        var item = _context.InventoryItems.FirstOrDefault(x => x.ID == id);
        if (item == null)
        {
            return NotFound();
        }
        _context.InventoryItems.Remove(item);
        _context.SaveChanges();
        return Ok(item);
    }
}
