using MasterFinder.ValueObjects.Exceptions;
using MasterFinder.ValueObjects.Validators;

namespace MasterFinder.Domain.Tests.Validators;

public class UsernameValidatorTests
{
    private readonly UsernameValidator _sut = new();

    [Theory]
    [InlineData("Ив")]
    [InlineData("Екатерина")]
    public void Validate_Valid_DoesNotThrow(string value)
    {
        _sut.Validate(value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Validate_NullOrWhiteSpace_Throws(string? value)
    {
        Assert.Throws<ArgumentNullOrWhiteSpaceException>(() => _sut.Validate(value!));
    }

    [Fact]
    public void Validate_TooShort_Throws()
    {
        Assert.Throws<ArgumentShortValueException>(() => _sut.Validate("И"));
    }

    [Fact]
    public void Validate_TooLong_Throws()
    {
        Assert.Throws<ArgumentLongValueException>(() => _sut.Validate(new string('a', 101)));
    }
}

public class PhoneNumberValidatorTests
{
    private readonly PhoneNumberValidator _sut = new();

    [Theory]
    [InlineData("+79941554567")]
    [InlineData("89941554567")]
    public void Validate_Valid_DoesNotThrow(string value)
    {
        _sut.Validate(value);
    }

    [Theory]
    [InlineData("abc")]
    [InlineData("+7 999")]
    public void Validate_InvalidFormat_Throws(string value)
    {
        Assert.Throws<FormatException>(() => _sut.Validate(value));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Validate_NullOrWhiteSpace_Throws(string? value)
    {
        Assert.Throws<ArgumentNullOrWhiteSpaceException>(() => _sut.Validate(value!));
    }
}

public class OrderTitleValidatorTests
{
    private readonly OrderTitleValidator _sut = new();

    [Fact]
    public void Validate_Valid_DoesNotThrow()
    {
        _sut.Validate("Заказ");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Validate_NullOrWhiteSpace_Throws(string? value)
    {
        Assert.Throws<ArgumentNullOrWhiteSpaceException>(() => _sut.Validate(value!));
    }

    [Fact]
    public void Validate_TooShort_Throws()
    {
        Assert.Throws<ArgumentShortValueException>(() => _sut.Validate("ab"));
    }

    [Fact]
    public void Validate_TooLong_Throws()
    {
        Assert.Throws<ArgumentLongValueException>(() => _sut.Validate(new string('a', 201)));
    }
}

public class OrderDescriptionValidatorTests
{
    private readonly OrderDescriptionValidator _sut = new();

    [Fact]
    public void Validate_Null_DoesNotThrow()
    {
        _sut.Validate(null);
    }

    [Fact]
    public void Validate_Valid_DoesNotThrow()
    {
        _sut.Validate("Описание");
    }

    [Fact]
    public void Validate_TooLong_Throws()
    {
        Assert.Throws<ArgumentLongValueException>(() => _sut.Validate(new string('a', 5001)));
    }
}

public class SpecializationValidatorTests
{
    private readonly SpecializationValidator _sut = new();

    [Fact]
    public void Validate_Valid_DoesNotThrow()
    {
        _sut.Validate("сантехник");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Validate_NullOrWhiteSpace_Throws(string? value)
    {
        Assert.Throws<ArgumentNullOrWhiteSpaceException>(() => _sut.Validate(value!));
    }

    [Fact]
    public void Validate_TooLong_Throws()
    {
        Assert.Throws<ArgumentLongValueException>(() => _sut.Validate(new string('a', 201)));
    }
}

public class ResponseCommentValidatorTests
{
    private readonly ResponseCommentValidator _sut = new();

    [Fact]
    public void Validate_Null_DoesNotThrow()
    {
        _sut.Validate(null);
    }

    [Fact]
    public void Validate_TooLong_Throws()
    {
        Assert.Throws<ArgumentLongValueException>(() => _sut.Validate(new string('a', 1001)));
    }
}

public class CancelReasonValidatorTests
{
    private readonly CancelReasonValidator _sut = new();

    [Fact]
    public void Validate_Null_DoesNotThrow()
    {
        _sut.Validate(null);
    }

    [Fact]
    public void Validate_TooLong_Throws()
    {
        Assert.Throws<ArgumentLongValueException>(() => _sut.Validate(new string('a', 501)));
    }
}