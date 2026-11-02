namespace LogiTrack;

public class InventoryItem
{
    public int ItemId { get; set; }

    public string? Name  { get; set; }

    public int Quantity { get; set; }

    public string? Location { get; set; }



    public void DisplayInfo()
    {
        ItemId = 1;
        Name = "Pallet Jack";
        Quantity = 12;
        Location = "Warehouse A";
    }
}
