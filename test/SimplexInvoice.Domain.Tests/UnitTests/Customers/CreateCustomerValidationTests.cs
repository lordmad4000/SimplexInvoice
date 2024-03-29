using SimplexInvoice.Domain.Customers;
using SimplexInvoice.Domain.Exceptions;
using SimplexInvoice.Domain.ValueObjects;
using System;
using Xunit;

namespace SimplexInvoice.Domain.Tests.UnitTests;
public class CreateCustomerValidationTests
{
    [Fact]
    public void Should_Not_Be_Throw_BusinessRuleValidationException()
    {
        //Arrange

        //Act
        var exception = Record.Exception(() =>
            Customer.Create("John",
                            "Connor",
                            Guid.NewGuid(),
                            "A37610482",
                            new Address("24, White Dog St.", "Kingston", "New York", "USA", "12401"),
                            new PhoneNumber("+01 718 222 2222"),
                            new EmailAddress("prueba@gmail.com")));

        //Assert
        Assert.Null(exception);
    }

    [Fact]
    public void Empty_FirstName_Should_Be_Throw_BusinessRuleValidationException()
    {
        // Arrange

        // Act & Assert
        Assert.Throws<BusinessRuleValidationException>(() =>
            Customer.Create("",
                            "Connor",
                            Guid.NewGuid(),
                            "A37610482",
                            new Address("24, White Dog St.", "Kingston", "New York", "USA", "12401"),
                            new PhoneNumber("+01 718 222 2222"),
                            new EmailAddress("prueba@gmail.com")));
    }

    [Fact]
    public void Null_FirstName_Should_Be_Throw_BusinessRuleValidationException()
    {
        // Arrange

        // Act & Assert
        Assert.Throws<BusinessRuleValidationException>(() =>
            Customer.Create(null,
                            "Connor",
                            Guid.NewGuid(),
                            "A37610482",
                            new Address("24, White Dog St.", "Kingston", "New York", "USA", "12401"),
                            new PhoneNumber("+01 718 222 2222"),
                            new EmailAddress("prueba@gmail.com")));
    }

    [Fact]
    public void FirstName_Length_Greater_Than_40_Should_Be_Throw_BusinessRuleValidationException()
    {
        // Arrange

        // Act & Assert
        Assert.Throws<BusinessRuleValidationException>(() =>
            Customer.Create("John_____________________________________",
                            "Connor",
                            Guid.NewGuid(),
                            "A37610482",
                            new Address("24, White Dog St.", "Kingston", "New York", "USA", "12401"),
                            new PhoneNumber("+01 718 222 2222"),
                            new EmailAddress("prueba@gmail.com")));
    }

    [Fact]
    public void Empty_LastName_Should_Be_Throw_BusinessRuleValidationException()
    {
        // Arrange

        // Act & Assert
        Assert.Throws<BusinessRuleValidationException>(() =>
            Customer.Create("John",
                            "",
                            Guid.NewGuid(),
                            "A37610482",
                            new Address("24, White Dog St.", "Kingston", "New York", "USA", "12401"),
                            new PhoneNumber("+01 718 222 2222"),
                            new EmailAddress("prueba@gmail.com")));
    }

    [Fact]
    public void Null_LastName_Should_Be_Throw_BusinessRuleValidationException()
    {
        // Arrange

        // Act & Assert
        Assert.Throws<BusinessRuleValidationException>(() =>
            Customer.Create("John",
                            null,
                            Guid.NewGuid(),
                            "A37610482",
                            new Address("24, White Dog St.", "Kingston", "New York", "USA", "12401"),
                            new PhoneNumber("+01 718 222 2222"),
                            new EmailAddress("prueba@gmail.com")));
    }

    [Fact]
    public void LastName_Length_Greater_Than_40_Should_Be_Throw_BusinessRuleValidationException()
    {
        // Arrange

        // Act & Assert
        Assert.Throws<BusinessRuleValidationException>(() =>
            Customer.Create("John",
                            "Connor___________________________________",
                            Guid.NewGuid(),
                            "A37610482",
                            new Address("24, White Dog St.", "Kingston", "New York", "USA", "12401"),
                            new PhoneNumber("+01 718 222 2222"),
                            new EmailAddress("prueba@gmail.com")));
    }

    [Fact]
    public void Empty_IdDocumentNumber_Should_Be_Throw_BusinessRuleValidationException()
    {
        // Arrange

        // Act & Assert
        Assert.Throws<BusinessRuleValidationException>(() =>
            Customer.Create("John",
                            "Connor",
                            Guid.NewGuid(),
                            "",
                            new Address("24, White Dog St.", "Kingston", "New York", "USA", "12401"),
                            new PhoneNumber("+01 718 222 2222"),
                            new EmailAddress("prueba@gmail.com")));
    }

    [Fact]
    public void Null_IdDocumentNumber_Should_Be_Throw_BusinessRuleValidationException()
    {
        // Arrange

        // Act & Assert
        Assert.Throws<BusinessRuleValidationException>(() =>
            Customer.Create("John",
                            "Connor",
                            Guid.NewGuid(),
                            null,
                            new Address("24, White Dog St.", "Kingston", "New York", "USA", "12401"),
                            new PhoneNumber("+01 718 222 2222"),
                            new EmailAddress("prueba@gmail.com")));
    }

    [Fact]
    public void IdDocumentNumber_Length_Greater_Than_40_Should_Be_Throw_BusinessRuleValidationException()
    {
        // Arrange

        // Act & Assert
        Assert.Throws<BusinessRuleValidationException>(() =>
            Customer.Create("John",
                            "Connor",
                            Guid.NewGuid(),
                            "A37610482                                ",
                            new Address("24, White Dog St.", "Kingston", "New York", "USA", "12401"),
                            new PhoneNumber("+01 718 222 2222"),
                            new EmailAddress("prueba@gmail.com")));
    }

    [Fact]
    public void Email_Length_Greater_Than_40_Should_Throw_BusinessRuleValidationException()
    {
        //Arrange

        //Act & Assert
        Assert.Throws<BusinessRuleValidationException>(() =>
            Customer.Create("John",
                            "Connor",
                            Guid.NewGuid(),
                            "A37610482",
                            new Address("24, White Dog St.", "Kingston", "New York", "USA", "12401"),
                            new PhoneNumber("+01 718 222 2222"),
                            new EmailAddress("pruebaaaaaaaaaaaaaaaaaaaaaaaaaa@gmail.com")));

    }

}
