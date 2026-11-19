using MasterFinder.Domain.Entities;
using MasterFinder.Domain.Enums;
using MasterFinder.ValueObjects;

namespace MasterFinder.Domain.Tests.Entities;

public class CustomerTests
{
    private static Customer CreateCustomer() =>
        new(new Username("Екатерина"), new PhoneNumber("+79941554567"));

    [Fact]
    public void Constructor_ValidData_SetsProperties()
    {
        var customer = CreateCustomer();

        Assert.Equal("Екатерина", customer.Username.Value);
        Assert.Equal("+79941554567", customer.Phone.Value);
        Assert.NotEqual(Guid.Empty, customer.Id);
        Assert.True(customer.CreatedAt <= DateTime.UtcNow);
    }

    [Fact]
    public void Constructor_NullUsername_Throws()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new Customer(null!, new PhoneNumber("+79941554567")));
    }

    [Fact]
    public void Constructor_NullPhone_Throws()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new Customer(new Username("Екатерина"), null!));
    }

    [Fact]
    public void CreateOrder_ValidData_AddsToOrdersAndReturnsOrder()
    {
        var customer = CreateCustomer();
        var title = new OrderTitle("Починить кран");

        var order = customer.CreateOrder(title, new OrderDescription("Вода капает"));

        Assert.Single(customer.Orders);
        Assert.Contains(order, customer.Orders);
        Assert.Equal(title, order.Title);
        Assert.Same(customer, order.Customer);
        Assert.Equal(OrderStatus.Open, order.Status);
    }

    [Fact]
    public void CreateOrder_WithoutDescription_UsesEmptyDescription()
    {
        var customer = CreateCustomer();

        var order = customer.CreateOrder(new OrderTitle("Починить кран"));

        Assert.NotNull(order.Description);
        Assert.Null(order.Description.Value);
    }

    [Fact]
    public void CreateOrder_NullTitle_Throws()
    {
        var customer = CreateCustomer();

        Assert.Throws<ArgumentNullException>(() => customer.CreateOrder(null!));
    }

    [Fact]
    public void CreateOrder_MultipleOrders_AllTracked()
    {
        var customer = CreateCustomer();

        customer.CreateOrder(new OrderTitle("Заказ 1"));
        customer.CreateOrder(new OrderTitle("Заказ 2"));

        Assert.Equal(2, customer.Orders.Count);
    }
}