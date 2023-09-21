using AutoMapper;
using MediatR;
using SimplexInvoice.Application.Common.Dto;
using SimplexInvoice.Application.Common.Interfaces.Persistance;
using SimplexInvoice.Application.IdDocumentTypes.Exceptions;
using SimplexInvoice.Domain.Exceptions;
using System.Threading;
using System.Threading.Tasks;

namespace SimplexInvoice.Application.IdDocumentTypes.Commands;
public class IdDocumentTypeUpdateHandler : IRequestHandler<IdDocumentTypeUpdateCommand, IdDocumentTypeDto>
{
    private readonly IIdDocumentTypeRepository _idDocumentTypeRepository;
    private readonly IMapper _mapper;
    private readonly ICustomLogger _logger;

    public IdDocumentTypeUpdateHandler(IIdDocumentTypeRepository idDocumentTypeRepository,
                                       IMapper mapper,
                                       ICustomLogger logger)
    {
        _idDocumentTypeRepository = idDocumentTypeRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IdDocumentTypeDto> Handle(IdDocumentTypeUpdateCommand request, CancellationToken cancellationToken)
    {
        var idDocumentType = await _idDocumentTypeRepository.GetAsync(c => c.Id == request.Id, cancellationToken, false)
            ?? throw new NotFoundException("IdDocumentType not found.");

        idDocumentType.Update(request.Name);
        IdDocumentTypeDto idDocumentTypeDto = _mapper.Map<IdDocumentTypeDto>(await _idDocumentTypeRepository.UpdateAsync(idDocumentType, cancellationToken));
        if (await _idDocumentTypeRepository.SaveChangesAsync(cancellationToken) == 0)
            throw new IdDocumentTypeUpdatingException($"Error updating the IdDocumentType.");

        _logger.Debug(@$"IdDocumentType Updated successfully with data: {idDocumentType}");

        return idDocumentTypeDto;
    }

}