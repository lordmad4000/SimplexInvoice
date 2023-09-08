using SimplexInvoice.Domain.IdDocumentTypes;
using System;
using System.ComponentModel.DataAnnotations;

namespace SimplexInvoice.Application.Common.Dto;

public class CompanyDto
{
    public Guid Id { get; set; }
    [Required(ErrorMessage = "Name is required.")]
    [StringLength(40, ErrorMessage = "Name cannot be longer than 40 characters.")]
    public string Name { get; set; }
    [Required(ErrorMessage = "IdDocumentTypeId is required.")]
    public Guid IdDocumentTypeId { get; set; }
    [Required(ErrorMessage = "IdDocumentNumber is required.")]
    [StringLength(40, ErrorMessage = "IdDocumentNumber cannot be longer than 40 characters.")]
    public string IdDocumentNumber { get; set; }
    [Required(ErrorMessage = "Street is required.")]
    [StringLength(40, ErrorMessage = "Street cannot be longer than 40 characters.")]
    public string Street { get; set; }
    [Required(ErrorMessage = "City is required.")]
    [StringLength(40, ErrorMessage = "City cannot be longer than 40 characters.")]
    public string City { get; set; }
    [Required(ErrorMessage = "State is required.")]
    [StringLength(40, ErrorMessage = "State cannot be longer than 40 characters.")]
    public string State { get; set; }
    [Required(ErrorMessage = "Country is required.")]
    [StringLength(40, ErrorMessage = "Country cannot be longer than 40 characters.")]
    public string Country { get; set; }
    [Required(ErrorMessage = "PostalCode is required.")]
    [StringLength(40, ErrorMessage = "PostalCode cannot be longer than 40 characters.")]
    public string PostalCode { get; set; }
    [Required(ErrorMessage = "Phone is required.")]
    public string Phone { get; set; }
    [Required(ErrorMessage = "Email is required.")]
    [StringLength(40, ErrorMessage = "Email cannot be longer than 40 characters.")]
    public string Email { get; set; }
    public IdDocumentType IdDocumentType { get; set; }
}