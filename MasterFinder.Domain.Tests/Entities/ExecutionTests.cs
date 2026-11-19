using MasterFinder.Domain.Entities;
using MasterFinder.Domain.Exceptions;
using MasterFinder.ValueObjects;

namespace MasterFinder.Domain.Tests.Entities;

public class ExecutionTests
{
    private static (Order order, Executor executor, Execution execution) Arrange()
    {
        var customer = new Customer(new Username("Екатерина"), new PhoneNumber("+79941554567"));
        var order = customer.CreateOrder(new OrderTitle("Заказ"));
        var executor = new Executor(new Username("Иван"),
            new PhoneNumber("+79991165233"), new Specialization("сантехник"));
        var response = order.AddResponse(executor);
        order.AcceptResponse(response, customer);
        order.StartExecution(executor);
        return (order, executor, order.Execution!);
    }

    [Fact]
    public void Constructor_SetsStartedAtAndNulls()
    {
        var (order, executor, execution) = Arrange();

        Assert.Same(order, execution.Order);
        Assert.Same(executor, execution.Executor);
        Assert.True(execution.StartedAt <= DateTime.UtcNow);
        Assert.Null(execution.CompletedAt);
        Assert.Null(execution.CancelledAt);
        Assert.NotNull(execution.CancelReason);
        Assert.Null(execution.CancelReason.Value);
    }

    [Fact]
    public void Complete_ByAssignedExecutor_ReturnsTrueAndSetsCompletedAt()
    {
        var (_, executor, execution) = Arrange();

        var result = execution.Complete(executor);

        Assert.True(result);
        Assert.NotNull(execution.CompletedAt);
    }

    [Fact]
    public void Complete_AlreadyCompleted_ReturnsFalse()
    {
        var (_, executor, execution) = Arrange();
        execution.Complete(executor);

        var result = execution.Complete(executor);

        Assert.False(result);
    }

    [Fact]
    public void Complete_WrongExecutor_Throws()
    {
        var (_, _, execution) = Arrange();
        var stranger = new Executor(new Username("Пётр"),
            new PhoneNumber("+79990000000"), new Specialization("электрик"));

        Assert.Throws<BusinessRuleViolationException>(() => execution.Complete(stranger));
    }

    [Fact]
    public void Complete_Cancelled_Throws()
    {
        var (_, executor, execution) = Arrange();
        execution.Cancel(executor);

        Assert.Throws<BusinessRuleViolationException>(() => execution.Complete(executor));
    }

    [Fact]
    public void Cancel_ByAssignedExecutor_ReturnsTrueAndSetsCancelledAt()
    {
        var (_, executor, execution) = Arrange();

        var result = execution.Cancel(executor, CancelReason.Create("Форс мажор"));

        Assert.True(result);
        Assert.NotNull(execution.CancelledAt);
        Assert.Equal("Форс мажор", execution.CancelReason!.Value);
    }

    [Fact]
    public void Cancel_WithoutReason_Works()
    {
        var (_, executor, execution) = Arrange();

        var result = execution.Cancel(executor);

        Assert.True(result);
        Assert.Null(execution.CancelReason);
    }

    [Fact]
    public void Cancel_AlreadyCancelled_ReturnsFalse()
    {
        var (_, executor, execution) = Arrange();
        execution.Cancel(executor);

        var result = execution.Cancel(executor);

        Assert.False(result);
    }

    [Fact]
    public void Cancel_Completed_Throws()
    {
        var (_, executor, execution) = Arrange();
        execution.Complete(executor);

        Assert.Throws<BusinessRuleViolationException>(() => execution.Cancel(executor));
    }

    [Fact]
    public void Cancel_WrongExecutor_Throws()
    {
        var (_, _, execution) = Arrange();
        var stranger = new Executor(new Username("Пётр"),
            new PhoneNumber("+79990000000"), new Specialization("электрик"));

        Assert.Throws<BusinessRuleViolationException>(() => execution.Cancel(stranger));
    }
}