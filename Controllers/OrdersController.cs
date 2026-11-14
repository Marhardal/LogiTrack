using LogiTrack;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
[Microsoft.AspNetCore.Authorization.Authorize]
public class OrdersController : ControllerBase
{
    public DBContext _context { get; set; }

    public OrdersController(DBContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Order>> GetOrders()
    {
        var orders = _context.Orders.Select(
        o => new Order
        {
            ID = o.ID,
            CustomerName = o.CustomerName,
            DatePlaced = o.DatePlaced,
            Items = o.Items.ToList()
        }
        ).ToList();
        return Ok(orders);
    }

    [HttpGet("{id}")]
    public ActionResult<Order> GetOrder(int id)
    {
        var order = _context.Orders.Include(o => o.Items).FirstOrDefault(o => o.ID == id);
        if (order == null)
        {
            return NotFound();
        }
        return Ok(order);
    }

    [HttpPost]
    public ActionResult<Order> CreateOrder([FromBody] Order order)
    {
        _context.Orders.Add(order);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetOrder), new { id = order.ID }, order);
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteOrder(int id)
    {
        var order = _context.Orders.FirstOrDefault(o => o.ID == id);
        if (order == null)
        {
            return NotFound();
        }
        _context.Orders.Remove(order);
        _context.SaveChanges();
        return NoContent();
    }
}

