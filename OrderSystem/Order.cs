namespace OrderSystem;

public class Order
{
    private readonly List<OrderItem> _items = new();

    public IReadOnlyList<OrderItem> Items => _items;

    public void AddItem(OrderItem item)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item));

        _items.Add(item);
    }

    public decimal GetSubtotal()
    {
        decimal subtotal = 0;

        foreach (var item in _items)
        {
            subtotal += item.GetTotalPrice();
        }

        return subtotal;
    }
}
