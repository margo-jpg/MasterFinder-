using MasterFinder.Domain.Entities;
using MasterFinder.Domain.Enums;
using MasterFinder.Domain.Exceptions;
using MasterFinder.ValueObjects;

namespace MasterFinder.Domain.Tests.Entities;

public class ExecutorTests
{
    private static Executor CreateExecutor(string name = "Иван", string phone = "+79991165233") =>
        new(new Username(name), new PhoneNumber(phone), new Specialization("сантехник"));

    private static Customer CreateCustomer() =>
        new(new Username("Екатерина"), new PhoneNumber("+79941554567"));

    [Fact]
    public void Constructor_ValidData_SetsProperties()
    {
        var executor = CreateExecutor();

        Assert.Equal("Иван", executor.Username.Value);
        Assert.Equal("+79991165233", executor.Phone.Value);
        Assert.Equal("сантехник", executor.Specialization.Value);
        Assert.NotEqual(Guid.Empty, executor.Id);
        Assert.True(executor.CreatedAt <= DateTime.UtcNow);
    }

    [Fact]
    public void Constructor_NullUsername_Throws()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new Executor(null!, new PhoneNumber("+79991165233"), new Specialization("сантехник")));
    }

    [Fact]
    public void Constructor_NullPhone_Throws()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new Executor(new Username("Иван"), null!, new Specialization("сантехник")));
    }

    [Fact]
    public void Constructor_NullSpecialization_Throws()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new Executor(new Username("Иван"), new PhoneNumber("+79991165233"), null!));
    }

    [Fact]
    public void RespondToOrder_OpenOrder_ReturnsResponseAndAddsToList()
    {
        var executor = CreateExecutor();
        var order = CreateCustomer().CreateOrder(new OrderTitle("Починить кран"));

        var response = executor.RespondToOrder(order, ResponseComment.Create("Могу завтра"));

        Assert.NotNull(response);
        Assert.Single(executor.Responses);
        Assert.Contains(response, executor.Responses);
        Assert.Equal(ResponseStatus.Pending, response.Status);
        Assert.Same(executor, response.Executor);
        Assert.Same(order, response.Order);
    }

    [Fact]
    public void RespondToOrder_WithoutComment_Works()
    {
        var executor = CreateExecutor();
        var order = CreateCustomer().CreateOrder(new OrderTitle("Починить кран"));

        var response = executor.RespondToOrder(order);

        Assert.NotNull(response.Comment);
        Assert.Null(response.Comment.Value);
    }

    [Fact]
    public void RespondToOrder_DuplicateResponse_Throws()
    {
        var executor = CreateExecutor();
        var order = CreateCustomer().CreateOrder(new OrderTitle("Починить кран"));
        executor.RespondToOrder(order);

        Assert.Throws<BusinessRuleViolationException>(() => executor.RespondToOrder(order));
    }

    [Fact]
    public void RespondToOrder_ClosedOrder_Throws()
    {
        var customer = CreateCustomer();
        var order = customer.CreateOrder(new OrderTitle("Починить кран"));
        var other = CreateExecutor("Пётр", "+79990000000");
        var resp = other.RespondToOrder(order);
        order.AcceptResponse(resp, customer);
        order.StartExecution(other);
        order.Complete(other);

        var executor = CreateExecutor();
        Assert.Throws<BusinessRuleViolationException>(() => executor.RespondToOrder(order));
    }
}