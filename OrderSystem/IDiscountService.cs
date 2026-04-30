namespace OrderSystem;

public interface IDiscountService
{
    decimal CalculateDiscount(decimal subtotal);
}