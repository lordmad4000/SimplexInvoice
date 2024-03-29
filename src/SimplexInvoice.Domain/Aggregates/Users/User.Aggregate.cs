using FluentValidation.Results;
using SimplexInvoice.Domain.Base;
using SimplexInvoice.Domain.Exceptions;
using SimplexInvoice.Domain.Users.Validations;
using SimplexInvoice.Domain.ValueObjects;
using System.Linq;
using System;

namespace SimplexInvoice.Domain.Users
{
    public partial class User : AggregateRoot
    {
        private User(Guid id) : base(id)
        {            
        }

        public static User Create(string emailAddress, string password, string firstName, string lastName)
        {
            var user = new User(Guid.NewGuid()) { CreationDate = DateTime.UtcNow };
            user.Update(emailAddress, password, firstName, lastName);

            return user;
        }

        public void Update(string emailAddress, string password, string firstName, string lastName)
        {
            EmailAddress = new EmailAddress(emailAddress);
            Password = password;
            FirstName = firstName;
            LastName = lastName;

            ValidationResult validator = new UpdateUserValidator().Validate(this);
            if (!validator.IsValid)
                throw new BusinessRuleValidationException(
                    string.Join(", ", validator.Errors.Select(x => x.ErrorMessage).ToArray()));
        }        

        public override string ToString()
        {
            return $"Id: {Id}, " +
                   $"Email: {EmailAddress.Address}, " +
                   $"Password: {Password}, " +
                   $"FirstName: {FirstName}, " +
                   $"LastName: {LastName}";
        }

    }
}