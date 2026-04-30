namespace OrderSystem;

public interface IDeliveryService
{
    decimal CalculateDelivery(decimal subtotal, DeliveryType deliveryType);
}