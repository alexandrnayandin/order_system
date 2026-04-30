namespace OrderSystem;

public class OrderCalculator
{
    private readonly IDiscountService _discountService;
    private readonly IDeliveryService _deliveryService;

    public OrderCalculator(IDiscountService discountService, IDeliveryService deliveryService)
    {
        _discountService = discountService ?? throw new ArgumentNullException(nameof(discountService));
        _deliveryService = deliveryService ?? throw new ArgumentNullException(nameof(deliveryService));
    }

    public decimal CalculateTotal(Order order, DeliveryType deliveryType)
    {
        if (order == null)
            throw new ArgumentNullException(nameof(order));

        if (order.Items.Count == 0)
            throw new InvalidOperationException("Заказ не может быть пустым.");

        decimal subtotal = order.GetSubtotal();
        decimal discount = _discountService.CalculateDiscount(subtotal);
        decimal delivery = _deliveryService.CalculateDelivery(subtotal, deliveryType);

        return subtotal - discount + delivery;
    }
}