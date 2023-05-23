using AutoMapper;
using Invoice.Application.AutoMapper;
using Invoice.Application.CQRS.Configuration.Commands.Register;
using Invoice.Application.Common.Dto;
using Invoice.Application.Common.Interfaces.Persistance;
using Invoice.Application.Interfaces;
using Invoice.Domain.Entities;
using Invoice.Domain.Exceptions;
using Moq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Threading;
using System;
using Xunit;

namespace Invoice.Application.Tests.UnitTests
{
    public class IdDocumentTypeRegisterHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IIdDocumentTypeRepository> _mockIdDocumentTypeRepository;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IValidatorService> _mockValidatorService;
        private readonly Mock<ICustomLogger> _mockLogger;

        public IdDocumentTypeRegisterHandlerTests()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new EntityDtoMappingProfile());
            });
            _mapper = mapperConfig.CreateMapper();
            _mockIdDocumentTypeRepository = new Mock<IIdDocumentTypeRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockValidatorService = new Mock<IValidatorService>();
            _mockLogger = new Mock<ICustomLogger>();
        }

        [Fact]
        public async Task UserRegisterCommand_Should_Not_Be_Null()
        {
            // Arrange
            var idDocumentType = GetIdDocumentType();
            var idDocumentTypeRegisterCommand = GetIdDocumentTypeRegisterCommand();
            _mockIdDocumentTypeRepository.Setup(x => x.AddAsync(It.IsAny<IdDocumentType>())).ReturnsAsync(idDocumentType);
            _mockIdDocumentTypeRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<IdDocumentType, bool>>>(), It.IsAny<bool>(), It.IsAny<string>())).ReturnsAsync(default(IdDocumentType));
            var idDocumentTypeRegisterHandler = new IdDocumentTypeRegisterHandler(_mockIdDocumentTypeRepository.Object,
                                                                                  _mockUnitOfWork.Object,
                                                                                  _mockValidatorService.Object,                                                                                
                                                                                  _mapper,
                                                                                  _mockLogger.Object);

            //Act
            IdDocumentTypeDto idDocumentTypeDto = await idDocumentTypeRegisterHandler.Handle(idDocumentTypeRegisterCommand, new CancellationToken());

            //Assert
            Assert.NotNull(idDocumentTypeDto);
        }

        [Fact]
        public async Task UserRegisterCommand_Should_Throw_An_EntityValidationException()
        {
            // Arrange
            var idDocumentType = GetIdDocumentType();
            var idDocumentTypeRegisterCommand = GetIdDocumentTypeRegisterCommand();
            _mockIdDocumentTypeRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<IdDocumentType, bool>>>(), It.IsAny<bool>(), It.IsAny<string>())).ReturnsAsync(idDocumentType);
            var idDocumentTypeRegisterHandler = new IdDocumentTypeRegisterHandler(_mockIdDocumentTypeRepository.Object,
                                                                                  _mockUnitOfWork.Object,
                                                                                  _mockValidatorService.Object,                                                                                
                                                                                  _mapper,
                                                                                  _mockLogger.Object);

            //Act & Assert
            await Assert.ThrowsAsync<EntityValidationException>(async () => await idDocumentTypeRegisterHandler.Handle(idDocumentTypeRegisterCommand, new CancellationToken()));
        }

        private IdDocumentType GetIdDocumentType()
        {
            return new IdDocumentType("DNI");
        }

        private IdDocumentTypeRegisterCommand GetIdDocumentTypeRegisterCommand()
        {
            return new IdDocumentTypeRegisterCommand("DNI");
        }

    }
}