using System;
using System.Linq;
using FluentValidation.Results;
using Invoice.Domain.Base;
using Invoice.Domain.Exceptions;
using Invoice.Domain.Validations;
using Invoice.Domain.ValueObjects;

namespace Invoice.Domain.Users
{
    public partial class User : IAggregateRoot
    {
        private User()
        {            
        }

        public static User Create(string emailAddress, string password, string firstName, string lastName)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                CreationDate = DateTime.UtcNow
            };
            user.Update(emailAddress, password, firstName, lastName);

            return user;
        }

        public void Update(string emailAddress, string password, string firstName, string lastName)
        {
            EmailAddress = EmailAddress.Create(emailAddress);
            Password = password;
            FirstName = firstName;
            LastName = lastName;

            ValidationResult validator = new RegisterUserValidator().Validate(this);
            if (!validator.IsValid)
                throw new EntityValidationException(
                    string.Join(", ", validator.Errors.Select(x => x.ErrorMessage).ToArray()));
        }        

        public override string ToString()
        {
            return @$"Id {Id}, 
                      Email {EmailAddress.Address}, 
                      Password {Password}, 
                      EncryptedPassword {Password}, 
                      FirstName {FirstName}, 
                      LastName {LastName}";
        }

    }
}