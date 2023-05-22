using AutoMapper;
using Invoice.Application.Common.Dto;
using Invoice.Application.Common.Interfaces.Persistance;
using MediatR;
using System.Threading.Tasks;
using System.Threading;

namespace Invoice.Application.CQRS.Users.Queries
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ICustomLogger _logger;

        public GetUserByIdQueryHandler(IUserRepository userRepository,
                                       IMapper mapper,
                                       ICustomLogger logger)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetAsync(c => c.Id == request.Id, true, $"Id=={request.Id}");
            _logger.Debug($"GetUserById with data: {user.ToString()}");

            return _mapper.Map<UserDto>(user);
        }
    }
}