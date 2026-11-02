namespace LogiTrack;

public class Order
{
    public int OrderId { get; set; }

    public string? CustomerName { get; set; }

    public DateTime DatePlaced  { get; set; }

    public List<InventoryItem> Items { get; set; } = new List<InventoryItem>();
    public List<Order> orders { get; set; } = new List<Order>();

    public bool AddItem(int id, string customerName, DateTime? datePlaced = null)
    {
        var order = new Order()
        {
            OrderId = id,
            CustomerName = customerName,
            DatePlaced = datePlaced ?? DateTime.Now
        };
        orders.Add(order);
        return true;
    }

    public bool RemoveItem(int id)
    {
        var order = orders.FirstOrDefault(o => o.OrderId == id);
        if (order != null)
        {
            orders.Remove(order);
        }
        return true;
    }

    public Order GetOrderSummary(int id)
    {
        var order = orders.FirstOrDefault(o => o.OrderId == id);
        return order ?? new Order();
    }

    public IEnumerable<Order> GetOrders()
    {
        var orderList = orders.ToList();
        return orderList;
    }
}
