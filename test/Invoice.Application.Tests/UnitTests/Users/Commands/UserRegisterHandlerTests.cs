using AutoMapper;
using Invoice.Application.AutoMapper;
using Invoice.Application.CQRS.Users.Commands;
using Invoice.Application.Common.Dto;
using Invoice.Application.Common.Interfaces.Persistance;
using Invoice.Application.Interfaces;
using Invoice.Domain.Entities;
using Invoice.Domain.Exceptions;
using Invoice.Domain.ValueObjects;
using Moq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Threading;
using System;
using Xunit;

namespace Invoice.Application.Tests.UnitTests.Users.Queries
{
    public class UserRegisterHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IValidatorService> _mockValidatorService;
        private readonly Mock<IPasswordService> _mockPasswordService;
        public UserRegisterHandlerTests()
        {
            var mapperConfig = new MapperConfiguration(cfg => 
            {
                cfg.AddProfile(new EntityDtoMappingProfile());
            });
            _mapper = mapperConfig.CreateMapper();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockValidatorService = new Mock<IValidatorService>();
            _mockPasswordService = new Mock<IPasswordService>();            
        }

        [Fact]
        public async Task UserRegisterCommand_Should_Not_Be_Null()
        {
            // Arrange
            var user = GetUser();
            var userRegisterCommand = GetUserRegisterCommand();
            _mockUserRepository.Setup(x => x.AddAsync(It.IsAny<User>())).ReturnsAsync(user);
            _mockUserRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<bool>(), It.IsAny<string>())).ReturnsAsync(default(User));
            _mockPasswordService.Setup(x => x.GeneratePassword(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>())).Returns("12345678");
            var userRegisterHandler = new UserRegisterHandler(_mockUserRepository.Object, 
                                                              _mockUnitOfWork.Object, 
                                                              _mockValidatorService.Object, 
                                                              _mockPasswordService.Object,
                                                              _mapper);

            //Act
            UserDto userDto = await userRegisterHandler.Handle(userRegisterCommand, new CancellationToken());

            //Assert
            Assert.NotNull(userDto);
        }

        [Fact]
        public async Task UserRegisterCommand_Should_Throw_An_EntityValidationException()
        {
            // Arrange
            var user = GetUser();
            var userRegisterCommand = GetUserRegisterCommand();
            _mockUserRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<bool>(), It.IsAny<string>())).ReturnsAsync(user);
            var userRegisterHandler = new UserRegisterHandler(_mockUserRepository.Object, 
                                                              _mockUnitOfWork.Object, 
                                                              _mockValidatorService.Object, 
                                                              _mockPasswordService.Object,
                                                              _mapper);

            //Act & Assert
            await Assert.ThrowsAsync<EntityValidationException>(async () => await userRegisterHandler.Handle(userRegisterCommand, new CancellationToken()));
        }

        private User GetUser()
        {
            return new User(new EmailAddress("jose@gmail.com"), "12345678", "jose", "antonio");
        }

        private UserRegisterCommand GetUserRegisterCommand()
        {
            return new UserRegisterCommand("jose@gmail.com", "12345678", "jose", "antonio");
        }

    }
}