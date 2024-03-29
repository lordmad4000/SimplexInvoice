﻿using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimplexInvoice.Api.Models.Request;
using SimplexInvoice.Application.Common.Dto;
using SimplexInvoice.Application.Common.Interfaces.Persistance;
using SimplexInvoice.Application.TaxRates.Commands;
using SimplexInvoice.Application.TaxRates.Queries;

namespace SimplexInvoice.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TaxRatesController : ApiController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly ICustomLogger _logger;
    public TaxRatesController(IMediator mediator,
                              IMapper mapper,
                              ICustomLogger logger)
    {
        _mediator = mediator;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet("GetById{id}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetTaxRateByIdQuery(id);
        TaxRateDto taxRateDto = await _mediator.Send(query, cancellationToken);

        return Ok(taxRateDto);
    }

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var query = new GetTaxRatesQuery();
        var taxRatesDto = await _mediator.Send(query, cancellationToken);

        return Ok(taxRatesDto);
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] TaxRateRegisterRequest taxRateRegisterRequest, CancellationToken cancellationToken)
    {
        EnsureModelStateIsValid();
        var taxRateRegisterCommand = _mapper.Map<TaxRateRegisterCommand>(taxRateRegisterRequest);
        var taxRateDto = await _mediator.Send(taxRateRegisterCommand, cancellationToken);
        string url = GetByIdUrl(taxRateDto.Id);

        return Created(url, taxRateDto);
    }

    [HttpPut("Update")]
    public async Task<IActionResult> Update([FromBody] TaxRateDto taxRateDto, CancellationToken cancellationToken)
    {
        EnsureModelStateIsValid();
        var taxRateUpdateCommand = _mapper.Map<TaxRateUpdateCommand>(taxRateDto);
        taxRateDto = await _mediator.Send(taxRateUpdateCommand, cancellationToken);

        return Ok(taxRateDto);
    }

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var taxRateRemoveCommand = new TaxRateRemoveCommand(id);
        bool result = await _mediator.Send(taxRateRemoveCommand, cancellationToken);

        return Ok(result);
    }

}