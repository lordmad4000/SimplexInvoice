using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using SimplexInvoice.Api.Controllers;
using SimplexInvoice.Api.Models.Request;
using SimplexInvoice.Api.Tests.IntegrationTests.Common;
using SimplexInvoice.Application.Common.Dto;
using SimplexInvoice.SimplexInvoice.Api;

namespace SimplexInvoice.Api.Tests.IntegrationTests;

public class CustomersFlow
{
    private Guid IdDocumentTypeId { get; set; } = Guid.Empty;
    private Guid CustomerId { get; set; } = Guid.Empty;
    public CustomersFlow()
    {
        IServiceCollection services = ServicesConfiguration.BuildDependencies();
        using (ServiceProvider serviceProvider = services.BuildServiceProvider())
        {

            var idDocumentTypeController = serviceProvider.GetRequiredService<IdDocumentTypesController>();
            var actionResult = idDocumentTypeController.GetAll(new CancellationToken());
            var idDocumentTypes = CustomConvert.OkResultTo<List<IdDocumentTypeDto>>(actionResult.Result);
            if (idDocumentTypes is not null)
                IdDocumentTypeId = idDocumentTypes.First().Id;
        }
    }

    [Fact]
    public async Task Register_Customer_Then_ModifyIt_Then_RemoveIt()
    {
        //Arrange
        IServiceCollection services = ServicesConfiguration.BuildDependencies();
        using (ServiceProvider serviceProvider = services.BuildServiceProvider())
        {
            var customersController = serviceProvider.GetRequiredService<CustomersController>();
            CustomerRegisterRequest customerRegisterRequest = GetCustomerRegisterRequest();

            //Act
            IActionResult actionResult = await customersController.Register(customerRegisterRequest, new CancellationToken());
            var customerDto = CustomConvert.CreatedResultTo<CustomerDto>(actionResult);

            //Assert
            Assert.NotNull(customerDto);
            if (customerDto is not null)
                CustomerId = customerDto.Id;
        };

        //Arrange
        services = ServicesConfiguration.BuildDependencies();
        using (ServiceProvider serviceProvider = services.BuildServiceProvider())
        {
            var customersController = serviceProvider.GetRequiredService<CustomersController>();
            CustomerUpdateRequest customerUpdateRequest = GetCustomerUpdateRequest();

            //Act
            IActionResult actionResult = await customersController.Update(customerUpdateRequest, new CancellationToken());
            var customerDto = CustomConvert.OkResultTo<CustomerDto>(actionResult);

            //Assert
            Assert.NotNull(customerDto);

            //Act
            actionResult = await customersController.Delete(CustomerId, new CancellationToken());
            bool result = CustomConvert.OkResultTo<bool>(actionResult);

            //Assert
            Assert.True(result);
        }
    }

    [Fact]
    public async Task Register_Customer_Then_GetById_Then_RemoveIt()
    {
        //Arrange
        IServiceCollection services = ServicesConfiguration.BuildDependencies();
        using (ServiceProvider serviceProvider = services.BuildServiceProvider())
        {
            var customersController = serviceProvider.GetRequiredService<CustomersController>();
            CustomerRegisterRequest customerRegisterRequest = GetCustomerRegisterRequest();

            //Act
            IActionResult actionResult = await customersController.Register(customerRegisterRequest, new CancellationToken());
            var customerDto = CustomConvert.CreatedResultTo<CustomerDto>(actionResult);

            //Assert
            Assert.NotNull(customerDto);
            if (customerDto is not null)
                CustomerId = customerDto.Id;

            //Act
            actionResult = await customersController.GetById(CustomerId, new CancellationToken());
            customerDto = CustomConvert.OkResultTo<CustomerDto>(actionResult);

            //Assert
            Assert.NotNull(customerDto);

            //Act
            actionResult = await customersController.Delete(CustomerId, new CancellationToken());
            bool result = CustomConvert.OkResultTo<bool>(actionResult);

            //Assert
            Assert.True(result);
        };
    }

    [Fact]
    public async Task Register_Customer_Then_GetAll_Then_RemoveIt()
    {
        //Arrange
        IServiceCollection services = ServicesConfiguration.BuildDependencies();
        using (ServiceProvider serviceProvider = services.BuildServiceProvider())
        {
            var customersController = serviceProvider.GetRequiredService<CustomersController>();
            CustomerRegisterRequest customerRegisterRequest = GetCustomerRegisterRequest();

            //Act
            IActionResult actionResult = await customersController.Register(customerRegisterRequest, new CancellationToken());
            var customerDto = CustomConvert.CreatedResultTo<CustomerDto>(actionResult);
            if (customerDto is not null)
                CustomerId = customerDto.Id;

            //Assert
            Assert.NotNull(customerDto);

            //Act
            actionResult = await customersController.GetAll(new CancellationToken());
            var customerDtos = CustomConvert.OkResultTo<List<CustomerDto>>(actionResult);

            //Assert
            Assert.NotEmpty(customerDtos);

            //Act
            actionResult = await customersController.Delete(CustomerId, new CancellationToken());
            bool result = CustomConvert.OkResultTo<bool>(actionResult);

            //Assert
            Assert.True(result);
        };
    }

    private CustomerRegisterRequest GetCustomerRegisterRequest() =>
        new CustomerRegisterRequest
        {
            FirstName = "TEST FIRSTNAME",
            LastName = "TEST LASTNAME",
            IdDocumentTypeId = IdDocumentTypeId,
            IdDocumentNumber = "TEST DOCUMENT NUMBER",
            Street = "TEST STREET",
            City = "TEST CITY",
            State = "TEST STATE",
            Country = "TEST COUNTRY",
            PostalCode = "TEST POSTAL CODE",
            Phone = "+01 718 222 2222",
            Email = "test@test.com"
        };

    private CustomerUpdateRequest GetCustomerUpdateRequest() =>
        new CustomerUpdateRequest
        {
            Id = CustomerId,
            FirstName = "TEST FIRSTNAME MOD",
            LastName = "TEST LASTNAME MOD",
            IdDocumentTypeId = IdDocumentTypeId,
            IdDocumentNumber = "TEST DOCUMENT NUMBER MOD",
            Street = "TEST STREET MOD",
            City = "TEST CITY MOD",
            State = "TEST STATE MOD",
            Country = "TEST COUNTRY MOD",
            PostalCode = "TEST POSTAL CODE MOD",
            Phone = "+01 718 222 2222",
            Email = "testmod@testmod.com"
        };
}