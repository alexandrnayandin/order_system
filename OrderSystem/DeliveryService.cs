namespace OrderSystem;

public enum DeliveryType
{
    Pickup,
    Standard,
    Express
}

public class DeliveryService : IDeliveryService
{
    public decimal CalculateDelivery(decimal subtotal, DeliveryType deliveryType)
    {
        if (subtotal < 0)
            throw new ArgumentException("Сумма заказа не может быть отрицательной.");

        return deliveryType switch
        {
            DeliveryType.Pickup => 0,
            DeliveryType.Standard => subtotal >= 3000 ? 0 : 300,
            DeliveryType.Express => 700,
            _ => throw new ArgumentException("Неизвестный тип доставки.")
        };
    }
}