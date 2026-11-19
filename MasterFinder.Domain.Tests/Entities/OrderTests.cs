using MasterFinder.Domain.Entities;
using MasterFinder.Domain.Enums;
using MasterFinder.Domain.Exceptions;
using MasterFinder.ValueObjects;

namespace MasterFinder.Domain.Tests.Entities;

public class OrderTests
{
    private static Customer CreateCustomer() =>
        new(new Username("Екатерина"), new PhoneNumber("+79941554567"));

    private static Executor CreateExecutor(string name = "Иван", string phone = "+79991165233") =>
        new(new Username(name), new PhoneNumber(phone), new Specialization("сантехник"));

    private static Order CreateOrder() =>
        CreateCustomer().CreateOrder(new OrderTitle("Починить кран"), new OrderDescription("Вода капает"));

    [Fact]
    public void Constructor_ValidData_SetsProperties()
    {
        var order = CreateOrder();

        Assert.Equal("Починить кран", order.Title.Value);
        Assert.Equal(OrderStatus.Open, order.Status);
        Assert.NotEqual(Guid.Empty, order.Id);
        Assert.NotNull(order.Customer);
        Assert.Empty(order.Responses);
        Assert.Null(order.Execution);
    }

    [Fact]
    public void Constructor_NullCustomer_Throws()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new Order(new OrderTitle("Заказ"), null, DateTime.UtcNow, null!));
    }

    [Fact]
    public void Constructor_NullTitle_Throws()
    {
        var customer = CreateCustomer();
        Assert.Throws<ArgumentNullException>(() =>
            new Order(null!, null, DateTime.UtcNow, customer));
    }

    [Fact]
    public void AddResponse_ValidExecutor_AddsResponse()
    {
        var order = CreateOrder();
        var executor = CreateExecutor();

        var response = order.AddResponse(executor, ResponseComment.Create("Возьмусь"));

        Assert.Single(order.Responses);
        Assert.Contains(response, order.Responses);
        Assert.Equal(ResponseStatus.Pending, response.Status);
    }

    [Fact]
    public void AddResponse_DuplicateExecutor_Throws()
    {
        var order = CreateOrder();
        var executor = CreateExecutor();
        order.AddResponse(executor);

        Assert.Throws<BusinessRuleViolationException>(() => order.AddResponse(executor));
    }

    [Fact]
    public void AcceptResponse_ValidData_ChangesStatusToAccepted()
    {
        var customer = CreateCustomer();
        var order = customer.CreateOrder(new OrderTitle("Заказ"));
        var executor = CreateExecutor();
        var response = order.AddResponse(executor);

        order.AcceptResponse(response, customer);

        Assert.Equal(ResponseStatus.Accepted, response.Status);
    }

    [Fact]
    public void AcceptResponse_NotOpenStatus_Throws()
    {
        var customer = CreateCustomer();
        var order = customer.CreateOrder(new OrderTitle("Заказ"));
        var executor = CreateExecutor();
        var response = order.AddResponse(executor);
        order.AcceptResponse(response, customer);
        order.StartExecution(executor);

        Assert.Throws<BusinessRuleViolationException>(() => order.AcceptResponse(response, customer));
    }

    [Fact]
    public void AcceptResponse_ForeignResponse_Throws()
    {
        var customer = CreateCustomer();
        var order = customer.CreateOrder(new OrderTitle("Заказ"));
        var executor = CreateExecutor();
        var response = order.AddResponse(executor);

        var foreignOrder = customer.CreateOrder(new OrderTitle("Другой"));
        var foreignExecutor = CreateExecutor("Пётр", "+79990000000");
        var foreignResponse = foreignOrder.AddResponse(foreignExecutor);

        Assert.Throws<NotFoundException>(() => order.AcceptResponse(foreignResponse, customer));
    }

    [Fact]
    public void StartExecution_AcceptedExecutor_ChangesStatusToInProgress()
    {
        var customer = CreateCustomer();
        var order = customer.CreateOrder(new OrderTitle("Заказ"));
        var executor = CreateExecutor();
        var response = order.AddResponse(executor);
        order.AcceptResponse(response, customer);

        order.StartExecution(executor);

        Assert.Equal(OrderStatus.InProgress, order.Status);
        Assert.NotNull(order.Execution);
        Assert.Same(executor, order.Execution.Executor);
    }

    [Fact]
    public void StartExecution_NotAcceptedExecutor_Throws()
    {
        var customer = CreateCustomer();
        var order = customer.CreateOrder(new OrderTitle("Заказ"));
        var executor = CreateExecutor();
        order.AddResponse(executor);

        Assert.Throws<BusinessRuleViolationException>(() => order.StartExecution(executor));
    }

    [Fact]
    public void StartExecution_NotOpenStatus_Throws()
    {
        var customer = CreateCustomer();
        var order = customer.CreateOrder(new OrderTitle("Заказ"));
        var executor = CreateExecutor();
        var response = order.AddResponse(executor);
        order.AcceptResponse(response, customer);
        order.StartExecution(executor);

        Assert.Throws<BusinessRuleViolationException>(() => order.StartExecution(executor));
    }

    [Fact]
    public void Complete_InProgress_ChangesStatusToCompleted()
    {
        var customer = CreateCustomer();
        var order = customer.CreateOrder(new OrderTitle("Заказ"));
        var executor = CreateExecutor();
        var response = order.AddResponse(executor);
        order.AcceptResponse(response, customer);
        order.StartExecution(executor);

        order.Complete(executor);

        Assert.Equal(OrderStatus.Completed, order.Status);
        Assert.NotNull(order.Execution!.CompletedAt);
    }

    [Fact]
    public void Complete_NotInProgress_Throws()
    {
        var order = CreateOrder();
        var executor = CreateExecutor();

        Assert.Throws<BusinessRuleViolationException>(() => order.Complete(executor));
    }

    [Fact]
    public void Complete_WrongExecutor_Throws()
    {
        var customer = CreateCustomer();
        var order = customer.CreateOrder(new OrderTitle("Заказ"));
        var executor = CreateExecutor();
        var response = order.AddResponse(executor);
        order.AcceptResponse(response, customer);
        order.StartExecution(executor);
        var stranger = CreateExecutor("Пётр", "+79990000000");

        Assert.Throws<BusinessRuleViolationException>(() => order.Complete(stranger));
    }

    [Fact]
    public void Cancel_OpenOrder_ChangesStatusToCancelled()
    {
        var order = CreateOrder();
        var executor = CreateExecutor();

        order.Cancel(executor, CancelReason.Create("Передумал"));

        Assert.Equal(OrderStatus.Cancelled, order.Status);
    }

    [Fact]
    public void Cancel_CompletedOrder_Throws()
    {
        var customer = CreateCustomer();
        var order = customer.CreateOrder(new OrderTitle("Заказ"));
        var executor = CreateExecutor();
        var response = order.AddResponse(executor);
        order.AcceptResponse(response, customer);
        order.StartExecution(executor);
        order.Complete(executor);

        Assert.Throws<BusinessRuleViolationException>(() => order.Cancel(executor));
    }

    [Fact]
    public void Cancel_InProgress_CancelsExecutionToo()
    {
        var customer = CreateCustomer();
        var order = customer.CreateOrder(new OrderTitle("Заказ"));
        var executor = CreateExecutor();
        var response = order.AddResponse(executor);
        order.AcceptResponse(response, customer);
        order.StartExecution(executor);

        order.Cancel(executor, CancelReason.Create("Форс мажор"));

        Assert.Equal(OrderStatus.Cancelled, order.Status);
        Assert.NotNull(order.Execution!.CancelledAt);
        Assert.Equal("Форс мажор", order.Execution.CancelReason!.Value);
    }
}