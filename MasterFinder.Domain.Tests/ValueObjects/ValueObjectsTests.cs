using MasterFinder.ValueObjects;
using MasterFinder.ValueObjects.Exceptions;

namespace MasterFinder.Domain.Tests.ValueObjects;

public class UsernameTests
{
    [Theory]
    [InlineData("Иван")]
    [InlineData("Екатерина")]
    [InlineData("  Пётр  ")]
    public void Constructor_Valid_StoresValue(string value)
    {
        var username = new Username(value);
        Assert.Equal(value, username.Value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_NullOrWhiteSpace_Throws(string? value)
    {
        Assert.Throws<ArgumentNullOrWhiteSpaceException>(() => new Username(value!));
    }

    [Fact]
    public void Constructor_TooShort_Throws()
    {
        Assert.Throws<ArgumentShortValueException>(() => new Username("И"));
    }

    [Fact]
    public void Constructor_TooLong_Throws()
    {
        Assert.Throws<ArgumentLongValueException>(() => new Username(new string('a', 101)));
    }

    [Fact]
    public void Equality_SameValue_IsEqual()
    {
        Assert.Equal(new Username("Иван"), new Username("Иван"));
    }

    [Fact]
    public void Equality_DifferentValue_NotEqual()
    {
        Assert.NotEqual(new Username("Иван"), new Username("Пётр"));
    }

    [Fact]
    public void ToString_ReturnsValue()
    {
        Assert.Equal("Иван", new Username("Иван").ToString());
    }
}

public class PhoneNumberTests
{
    [Theory]
    [InlineData("+79941554567")]
    [InlineData("89941554567")]
    [InlineData("+1234567890")]
    public void Constructor_Valid_StoresValue(string value)
    {
        var phone = new PhoneNumber(value);
        Assert.Equal(value, phone.Value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_NullOrWhiteSpace_Throws(string? value)
    {
        Assert.Throws<ArgumentNullOrWhiteSpaceException>(() => new PhoneNumber(value!));
    }

    [Theory]
    [InlineData("abc")]
    [InlineData("+7 999 116 52 33")]
    [InlineData("12345")]
    public void Constructor_InvalidFormat_Throws(string value)
    {
        Assert.Throws<FormatException>(() => new PhoneNumber(value));
    }
}

public class OrderTitleTests
{
    [Theory]
    [InlineData("Кран")]
    [InlineData("Починить кран на кухне")]
    public void Constructor_Valid_StoresValue(string value)
    {
        var title = new OrderTitle(value);
        Assert.Equal(value, title.Value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_NullOrWhiteSpace_Throws(string? value)
    {
        Assert.Throws<ArgumentNullOrWhiteSpaceException>(() => new OrderTitle(value!));
    }

    [Fact]
    public void Constructor_TooShort_Throws()
    {
        Assert.Throws<ArgumentShortValueException>(() => new OrderTitle("ab"));
    }

    [Fact]
    public void Constructor_TooLong_Throws()
    {
        Assert.Throws<ArgumentLongValueException>(() => new OrderTitle(new string('a', 201)));
    }

    [Fact]
    public void Create_ReturnsSameValue()
    {
        Assert.Equal("Заказ", OrderTitle.Create("Заказ").Value);
    }
}

public class OrderDescriptionTests
{
    [Fact]
    public void Constructor_NullAllowed()
    {
        Assert.Null(new OrderDescription(null).Value);
    }

    [Fact]
    public void Constructor_Valid_StoresValue()
    {
        Assert.Equal("Описание", new OrderDescription("Описание").Value);
    }

    [Fact]
    public void Constructor_TooLong_Throws()
    {
        Assert.Throws<ArgumentLongValueException>(() => new OrderDescription(new string('a', 5001)));
    }
}

public class SpecializationTests
{
    [Fact]
    public void Constructor_Valid_StoresValue()
    {
        Assert.Equal("сантехник", new Specialization("сантехник").Value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_NullOrWhiteSpace_Throws(string? value)
    {
        Assert.Throws<ArgumentNullOrWhiteSpaceException>(() => new Specialization(value!));
    }

    [Fact]
    public void Constructor_TooLong_Throws()
    {
        Assert.Throws<ArgumentLongValueException>(() => new Specialization(new string('a', 201)));
    }
}

public class ResponseCommentTests
{
    [Fact]
    public void Constructor_NullAllowed()
    {
        Assert.Null(new ResponseComment(null).Value);
    }

    [Fact]
    public void Create_ReturnsInstance()
    {
        Assert.Equal("Текст", ResponseComment.Create("Текст").Value);
    }

    [Fact]
    public void Constructor_TooLong_Throws()
    {
        Assert.Throws<ArgumentLongValueException>(() => new ResponseComment(new string('a', 1001)));
    }
}

public class CancelReasonTests
{
    [Fact]
    public void Constructor_NullAllowed()
    {
        Assert.Null(new CancelReason(null).Value);
    }

    [Fact]
    public void Create_ReturnsInstance()
    {
        Assert.Equal("Форс мажор", CancelReason.Create("Форс мажор").Value);
    }

    [Fact]
    public void Constructor_TooLong_Throws()
    {
        Assert.Throws<ArgumentLongValueException>(() => new CancelReason(new string('a', 501)));
    }
}