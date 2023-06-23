using SimplexInvoice.Domain.Exceptions;
using SimplexInvoice.Domain.Products;
using System;
using Xunit;

namespace SimplexInvoice.Domain.Tests.UnitTests;
public class UpdateProductValidationTests
{
    [Fact]
    public void Should_Not_Be_Throw_BusinessRuleValidationException()
    {
        //Arrange
        var product = GetProduct();

        //Act
        var exception = Record.Exception(() =>
            product.Update("LASAÑA BOLOGNESA",
                           "LASAÑA BOLOGNESA",
                           2.60,
                           "USD",
                           Guid.NewGuid()));

        //Assert
        Assert.Null(exception);
    }

    [Fact]
    public void Empty_Name_Should_Be_Throw_BusinessRuleValidationException()
    {
        //Arrange
        var product = GetProduct();

        //Act & Assert
        Assert.Throws<BusinessRuleValidationException>(() =>
            product.Update("",
                           "LASAÑA BOLOGNESA",
                           2.60,
                           "USD",
                           Guid.NewGuid()));
    }

    [Fact]
    public void Null_Name_Should_Be_Throw_BusinessRuleValidationException()
    {
        //Arrange
        var product = GetProduct();

        //Act & Assert
        Assert.Throws<BusinessRuleValidationException>(() =>
            product.Update(null,
                           "LASAÑA BOLOGNESA",
                           2.60,
                           "USD",
                           Guid.NewGuid()));
    }

    [Fact]
    public void Name_Length_Greater_Than_40_Should_Be_Throw_BusinessRuleValidationException()
    {
        //Arrange
        var product = GetProduct();

        //Act & Assert
        Assert.Throws<BusinessRuleValidationException>(() =>
            product.Update("LASAÑA BOLOGNESA_________________________",
                           "LASAÑA BOLOGNESA",
                           2.60,
                           "USD",
                           Guid.NewGuid()));
    }

    [Fact]
    public void Empty_Description_Should_Be_Throw_BusinessRuleValidationException()
    {
        //Arrange
        var product = GetProduct();

        //Act & Assert
        Assert.Throws<BusinessRuleValidationException>(() =>
            product.Update("LASAÑA BOLOGNESA",
                           "",
                           2.60,
                           "USD",
                           Guid.NewGuid()));
    }

    [Fact]
    public void Null_Description_Should_Be_Throw_BusinessRuleValidationException()
    {
        //Arrange
        var product = GetProduct();

        //Act & Assert
        Assert.Throws<BusinessRuleValidationException>(() =>
            product.Update("LASAÑA BOLOGNESA",
                           null,
                           2.60,
                           "USD",
                           Guid.NewGuid()));
    }

    [Fact]
    public void Description_Length_Greater_Than_40_Should_Be_Throw_BusinessRuleValidationException()
    {
        //Arrange
        var product = GetProduct();

        //Act & Assert
        Assert.Throws<BusinessRuleValidationException>(() =>
            product.Update("LASAÑA BOLOGNESA",
                           "LASAÑA BOLOGNESA_________________________",
                           2.60,
                           "USD",
                           Guid.NewGuid()));
    }

    [Fact]
    public void UnitPriceAmount_Lesser_Than_0_Should_Be_Throw_BusinessRuleValidationException()
    {
        //Arrange
        var product = GetProduct();

        //Act & Assert
        Assert.Throws<BusinessRuleValidationException>(() =>
            product.Update("LASAÑA BOLOGNESA",
                           "LASAÑA BOLOGNESA",
                           -1.50,
                           "USD",
                           Guid.NewGuid()));
    }

    private static Product GetProduct() =>
        Product.Create("FAIRY ULTRA",
                       "FAIRY ULTRA",
                       3.25,
                       "EUR",
                       Guid.NewGuid());

}
