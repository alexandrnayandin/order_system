namespace OrderSystem;

public class DiscountService : IDiscountService
{
    public decimal CalculateDiscount(decimal subtotal)
    {
        if (subtotal < 0)
            throw new ArgumentException("Сумма заказа не может быть отрицательной.");

        if (subtotal >= 10000)
            return subtotal * 0.15m;

        if (subtotal >= 5000)
            return subtotal * 0.10m;

        return 0;
    }
}