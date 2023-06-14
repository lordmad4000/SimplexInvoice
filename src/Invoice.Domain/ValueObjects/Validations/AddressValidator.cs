using FluentValidation;

namespace Invoice.Domain.ValueObjects.Validations;

public class AddressValidator : AbstractValidator<Address>
{
    public AddressValidator()
    {
        ValidateStreet();
        ValidateCity();
        ValidateCountry();
        ValidatePostalCode();
    }
    
    public void ValidateStreet()
    {
        RuleFor(c => c.Street).NotEmpty()
                              .NotNull()
                              .Length(1, 40);
    }

    public void ValidateCity()
    {
        RuleFor(c => c.City).NotEmpty()
                            .NotNull()
                            .Length(1, 40);
    }

    public void ValidateCountry()
    {
        RuleFor(c => c.Country).NotEmpty()
                               .NotNull()
                               .Length(1, 40);
    }

    public void ValidatePostalCode()
    {
        RuleFor(c => c.PostalCode).NotEmpty()
                                  .NotNull()
                                  .Length(1, 40);
    }

}