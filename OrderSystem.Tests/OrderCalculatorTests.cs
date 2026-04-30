using Moq;
using Xunit;

namespace OrderSystem.Tests;

public class OrderCalculatorTests
{
    [Fact]
    public void Product_WithEmptyName_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new Product("", 100));
    }

    [Fact]
    public void Product_WithValidData_SetsProperties()
    {
        var product = new Product("Книга", 100);

        Assert.Equal("Книга", product.Name);
        Assert.Equal(100, product.Price);
    }

    [Fact]
    public void Product_WithZeroPrice_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new Product("Книга", 0));
    }

    [Fact]
    public void OrderItem_WithNullProduct_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new OrderItem(null!, 1));
    }

    [Fact]
    public void OrderItem_WithZeroQuantity_ThrowsArgumentException()
    {
        var product = new Product("Книга", 500);

        Assert.Throws<ArgumentException>(() => new OrderItem(product, 0));
    }

    [Fact]
    public void OrderItem_GetTotalPrice_ReturnsCorrectTotal()
    {
        var product = new Product("Книга", 500);
        var item = new OrderItem(product, 3);

        var result = item.GetTotalPrice();

        Assert.Equal(1500, result);
    }

    [Fact]
    public void Order_AddNullItem_ThrowsArgumentNullException()
    {
        var order = new Order();

        Assert.Throws<ArgumentNullException>(() => order.AddItem(null!));
    }

    [Fact]
    public void Order_GetSubtotal_ReturnsSumOfItems()
    {
        var order = new Order();
        order.AddItem(new OrderItem(new Product("Книга", 500), 2));
        order.AddItem(new OrderItem(new Product("Ручка", 100), 3));

        var result = order.GetSubtotal();

        Assert.Equal(1300, result);
    }

    [Fact]
    public void DiscountService_WithNegativeSubtotal_ThrowsArgumentException()
    {
        var service = new DiscountService();

        Assert.Throws<ArgumentException>(() => service.CalculateDiscount(-1));
    }

    [Fact]
    public void DiscountService_WithSubtotalGreaterThan10000_Returns15PercentDiscount()
    {
        var service = new DiscountService();

        var result = service.CalculateDiscount(10000);

        Assert.Equal(1500, result);
    }

    [Fact]
    public void DiscountService_WithSubtotalGreaterThan5000_Returns10PercentDiscount()
    {
        var service = new DiscountService();

        var result = service.CalculateDiscount(5000);

        Assert.Equal(500, result);
    }

    [Fact]
    public void DiscountService_WithSmallSubtotal_ReturnsZeroDiscount()
    {
        var service = new DiscountService();

        var result = service.CalculateDiscount(1000);

        Assert.Equal(0, result);
    }

    [Fact]
    public void DeliveryService_WithNegativeSubtotal_ThrowsArgumentException()
    {
        var service = new DeliveryService();

        Assert.Throws<ArgumentException>(() => service.CalculateDelivery(-1, DeliveryType.Standard));
    }

    [Fact]
    public void DeliveryService_WithPickup_ReturnsZero()
    {
        var service = new DeliveryService();

        var result = service.CalculateDelivery(1000, DeliveryType.Pickup);

        Assert.Equal(0, result);
    }

    [Fact]
    public void DeliveryService_WithStandardAndSubtotalLessThan3000_Returns300()
    {
        var service = new DeliveryService();

        var result = service.CalculateDelivery(1000, DeliveryType.Standard);

        Assert.Equal(300, result);
    }

    [Fact]
    public void DeliveryService_WithStandardAndSubtotalGreaterThan3000_ReturnsZero()
    {
        var service = new DeliveryService();

        var result = service.CalculateDelivery(3000, DeliveryType.Standard);

        Assert.Equal(0, result);
    }

    [Fact]
    public void DeliveryService_WithExpress_Returns700()
    {
        var service = new DeliveryService();

        var result = service.CalculateDelivery(1000, DeliveryType.Express);

        Assert.Equal(700, result);
    }

    [Fact]
    public void DeliveryService_WithUnknownDeliveryType_ThrowsArgumentException()
    {
        var service = new DeliveryService();

        Assert.Throws<ArgumentException>(() =>
            service.CalculateDelivery(1000, (DeliveryType)999));
    }

    [Fact]
    public void OrderCalculator_WithNullDiscountService_ThrowsArgumentNullException()
    {
        var deliveryService = new DeliveryService();

        Assert.Throws<ArgumentNullException>(() =>
            new OrderCalculator(null!, deliveryService));
    }

    [Fact]
    public void OrderCalculator_WithNullDeliveryService_ThrowsArgumentNullException()
    {
        var discountService = new DiscountService();

        Assert.Throws<ArgumentNullException>(() =>
            new OrderCalculator(discountService, null!));
    }

    [Fact]
    public void OrderCalculator_WithNullOrder_ThrowsArgumentNullException()
    {
        var calculator = new OrderCalculator(new DiscountService(), new DeliveryService());

        Assert.Throws<ArgumentNullException>(() =>
            calculator.CalculateTotal(null!, DeliveryType.Standard));
    }

    [Fact]
    public void OrderCalculator_WithEmptyOrder_ThrowsInvalidOperationException()
    {
        var calculator = new OrderCalculator(new DiscountService(), new DeliveryService());
        var order = new Order();

        Assert.Throws<InvalidOperationException>(() =>
            calculator.CalculateTotal(order, DeliveryType.Standard));
    }

    [Fact]
    public void OrderCalculator_CalculateTotal_UsesMockServices()
    {
        var order = new Order();
        order.AddItem(new OrderItem(new Product("Книга", 1000), 2));

        var discountMock = new Mock<IDiscountService>();
        discountMock.Setup(service => service.CalculateDiscount(2000))
            .Returns(200);

        var deliveryMock = new Mock<IDeliveryService>();
        deliveryMock.Setup(service => service.CalculateDelivery(2000, DeliveryType.Standard))
            .Returns(300);

        var calculator = new OrderCalculator(discountMock.Object, deliveryMock.Object);

        var result = calculator.CalculateTotal(order, DeliveryType.Standard);

        Assert.Equal(2100, result);

        discountMock.Verify(service => service.CalculateDiscount(2000), Times.Once);
        deliveryMock.Verify(service => service.CalculateDelivery(2000, DeliveryType.Standard), Times.Once);
    }
}
