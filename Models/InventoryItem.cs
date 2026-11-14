using Microsoft.EntityFrameworkCore;

namespace LogiTrack;

public class InventoryItem
{
    public int ID { get; set; }

    public string? Name  { get; set; }

    public int Quantity { get; set; }

    public string? Location { get; set; }

    public int? OrderID { get; set; }

    public Order? Order { get; set; }


    public void DisplayInfo()
    {
        ID = 1;
        Name = "Pallet Jack";
        Quantity = 12;
        Location = "Warehouse A";
    }

    public bool add(InventoryItem item)
    {
        using (var context = new DBContext())
        {
            context.InventoryItems.Add(item);
            context.SaveChanges();
        }
        return true;
    }
}
