namespace OrderSystem;

public class Product
{
    public string Name { get; }
    public decimal Price { get; }

    public Product(string name, decimal price)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Название товара не может быть пустым.");

        if (price <= 0)
            throw new ArgumentException("Цена товара должна быть больше нуля.");

        Name = name;
        Price = price;
    }
}