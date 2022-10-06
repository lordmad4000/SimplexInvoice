using AutoMapper;
using Invoice.Application.Common.Dto;
using Invoice.Application.Common.Interfaces.Persistance;
using Invoice.Application.Interfaces;
using Invoice.Domain.Entities;
using Invoice.Domain.Exceptions;
using Invoice.Domain.Validations;
using Invoice.Domain.ValueObjects;
using MediatR;
using System.Threading.Tasks;
using System.Threading;

namespace Invoice.Application.CQRS.Authentication.Commands
{

    public class AuthenticationRegisterHandler : IRequestHandler<AuthenticationRegisterCommand, UserDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidatorService _validatorService;
        private readonly IPasswordService _passwordService;
        private readonly IMapper _mapper;
        private readonly ICustomLogger _logger;

        public AuthenticationRegisterHandler(IUserRepository userRepository,                        
                                             IUnitOfWork unitOfWork,
                                             IValidatorService validatorService,
                                             IPasswordService passwordService,
                                             IMapper mapper,
                                             ICustomLogger logger)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _validatorService = validatorService;
            _passwordService = passwordService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<UserDto> Handle(AuthenticationRegisterCommand request, CancellationToken cancellationToken)
        {            
            await Validate(request);
            var encryptedPassword = _passwordService.GeneratePassword(request.Email, request.Password, 16);
            var user = new User(new EmailAddress(request.Email), encryptedPassword, request.FirstName, request.LastName);
            user = await _userRepository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();
            _logger.Debug(@$"Authentication Register with data: 
                             Id {user.Id}, 
                             Email {user.EmailAddress.Address}, 
                             Password {request.Password}, 
                             EncryptedPassword {user.Password}, 
                             FirstName {user.FirstName}, 
                             LastName {user.LastName}");

            return _mapper.Map<UserDto>(user);
        }

        private async Task Validate(AuthenticationRegisterCommand request)
        {
            var user = await _userRepository.GetAsync(c => c.EmailAddress.Address == request.Email, false);
            if (user != null)
                throw new EntityValidationException("Email address already exists.");

            user = new User(new EmailAddress(request.Email),
                            request.Password, 
                            request.FirstName, 
                            request.LastName);
            _validatorService.ValidateModel(new RegisterUserValidator().Validate(user));                                
        }

    }
}