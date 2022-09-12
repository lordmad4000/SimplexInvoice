using AutoMapper;
using Invoice.Application.AutoMapper;
using Invoice.Application.CQRS.Users.Commands;
using Invoice.Application.Common.Dto;
using Invoice.Application.Common.Interfaces.Persistance;
using Invoice.Application.Interfaces;
using Invoice.Domain.Entities;
using Invoice.Domain.ValueObjects;
using Moq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Threading;
using System;
using Xunit;

namespace Invoice.Application.Tests.UnitTests.Users.Queries
{
    public class UserUpdateHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IValidatorService> _mockValidatorService;
        private readonly Mock<IPasswordService> _mockPasswordService;
        public UserUpdateHandlerTests()
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
        public async Task UserUpdateCommand_Should_Not_Be_Null()
        {
            // Arrange
            var user = GetUser();
            var userUpdateCommand = GetUserUpdateCommand();
            _mockUserRepository.Setup(x => x.DeleteAsync(It.IsAny<Guid>())).ReturnsAsync(true);
            _mockUserRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<bool>(), It.IsAny<string>())).ReturnsAsync(user);
            _mockPasswordService.Setup(x => x.GeneratePassword(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>())).Returns("12345678");
            _mockUnitOfWork.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);
            var userUpdateHandler = new UserUpdateHandler(_mockUserRepository.Object, 
                                                          _mockUnitOfWork.Object, 
                                                          _mockValidatorService.Object, 
                                                          _mockPasswordService.Object,
                                                          _mapper);

            //Act
            UserDto userDto = await userUpdateHandler.Handle(userUpdateCommand, new CancellationToken());

            //Assert
            Assert.NotNull(userDto);
        }

        [Fact]
        public async Task UserRegisterCommand_Should_Be_Null()
        {
            // Arrange
            var user = GetUser();
            var userUpdateCommand = GetUserUpdateCommand();
            _mockUserRepository.Setup(x => x.UpdateAsync(It.IsAny<User>())).ReturnsAsync(user);
            _mockUserRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<bool>(), It.IsAny<string>())).ReturnsAsync(user);
            _mockPasswordService.Setup(x => x.GeneratePassword(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>())).Returns("12345678");
            _mockUnitOfWork.Setup(x => x.SaveChangesAsync()).ReturnsAsync(0);
            var userUpdateHandler = new UserUpdateHandler(_mockUserRepository.Object, 
                                                          _mockUnitOfWork.Object, 
                                                          _mockValidatorService.Object, 
                                                          _mockPasswordService.Object,
                                                          _mapper);

            //Act
            UserDto userDto = await userUpdateHandler.Handle(userUpdateCommand, new CancellationToken());

            //Assert
            Assert.Null(userDto);
        }

        private User GetUser()
        {
            return new User(new EmailAddress("jose@gmail.com"), "12345678", "jose", "antonio");
        }

        private UserUpdateCommand GetUserUpdateCommand()
        {
            return new UserUpdateCommand(new Guid("a0769b81-2e5d-4e75-9cfd-e92c38fd70ca"), "jose@gmail.com", "12345678", "jose", "antonio");            
        }

    }
}