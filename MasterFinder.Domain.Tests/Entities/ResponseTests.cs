using MasterFinder.Domain.Entities;
using MasterFinder.Domain.Enums;
using MasterFinder.Domain.Exceptions;
using MasterFinder.ValueObjects;

namespace MasterFinder.Domain.Tests.Entities;

public class ResponseTests
{
    private static Customer CreateCustomer() =>
        new(new Username("Екатерина"), new PhoneNumber("+79941554567"));

    private static Executor CreateExecutor(string name = "Иван", string phone = "+79991165233") =>
        new(new Username(name), new PhoneNumber(phone), new Specialization("сантехник"));

    private static (Customer customer, Order order, Executor executor, Response response) Arrange()
    {
        var customer = CreateCustomer();
        var order = customer.CreateOrder(new OrderTitle("Заказ"));
        var executor = CreateExecutor();
        var response = order.AddResponse(executor);
        return (customer, order, executor, response);
    }

    [Fact]
    public void Constructor_SetsPendingAndDates()
    {
        var (_, order, executor, response) = Arrange();

        Assert.Equal(ResponseStatus.Pending, response.Status);
        Assert.Same(order, response.Order);
        Assert.Same(executor, response.Executor);
        Assert.True(response.CreatedAt <= DateTime.UtcNow);
    }

    [Fact]
    public void Withdraw_ByAuthor_ChangesStatusToWithdrawn()
    {
        var (_, _, executor, response) = Arrange();

        response.Withdraw(executor);

        Assert.Equal(ResponseStatus.Withdrawn, response.Status);
    }

    [Fact]
    public void Withdraw_ByStranger_Throws()
    {
        var (_, _, _, response) = Arrange();
        var stranger = CreateExecutor("Пётр", "+79990000000");

        Assert.Throws<BusinessRuleViolationException>(() => response.Withdraw(stranger));
    }

    [Fact]
    public void Withdraw_NotPending_Throws()
    {
        var (_, _, executor, response) = Arrange();
        response.Withdraw(executor);

        Assert.Throws<BusinessRuleViolationException>(() => response.Withdraw(executor));
    }

    [Fact]
    public void Accept_ByOrderCustomer_ChangesStatusToAccepted()
    {
        var (customer, _, _, response) = Arrange();

        response.Accept(customer);

        Assert.Equal(ResponseStatus.Accepted, response.Status);
    }

    [Fact]
    public void Accept_ByStrangerCustomer_Throws()
    {
        var (_, _, _, response) = Arrange();
        var stranger = CreateCustomer();

        Assert.Throws<BusinessRuleViolationException>(() => response.Accept(stranger));
    }

    [Fact]
    public void Accept_NotPending_Throws()
    {
        var (customer, _, _, response) = Arrange();
        response.Accept(customer);

        Assert.Throws<BusinessRuleViolationException>(() => response.Accept(customer));
    }

    [Fact]
    public void Reject_ByOrderCustomer_ChangesStatusToRejected()
    {
        var (customer, _, _, response) = Arrange();

        response.Reject(customer);

        Assert.Equal(ResponseStatus.Rejected, response.Status);
    }

    [Fact]
    public void Reject_ByStranger_Throws()
    {
        var (_, _, _, response) = Arrange();
        var stranger = CreateCustomer();

        Assert.Throws<BusinessRuleViolationException>(() => response.Reject(stranger));
    }

    [Fact]
    public void Reject_NotPending_Throws()
    {
        var (customer, _, _, response) = Arrange();
        response.Reject(customer);

        Assert.Throws<BusinessRuleViolationException>(() => response.Reject(customer));
    }
}