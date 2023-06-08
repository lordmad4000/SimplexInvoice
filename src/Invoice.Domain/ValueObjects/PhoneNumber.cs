using Invoice.Domain.Exceptions;
using System.Text.RegularExpressions;
using System;

namespace Invoice.Domain.ValueObjects
{
    public class PhoneNumber
    {
        public string Phone { get; private set; } = string.Empty;
        private PhoneNumber()
        {
        }

        public PhoneNumber(string phoneNumber)
        {
            Phone = phoneNumber;
            Validate();
        }

        public static PhoneNumber Create(string phone)
        {
            var phoneNumber = new PhoneNumber(phone);
            phoneNumber.Validate();

            return phoneNumber;
        }

        private void Validate()
        {
            if (!Validate(this.Phone))
                throw new NotValidPhoneNumberException(String.Format($"{this.Phone} is not valid phone number."));
        }

        private bool Validate(string phoneNumber)
        {
            string patternPhoneCountry = @"^(?<plus>[\+])(?<country>[1-9][0-9]{0,2})(?<area>0?[1-9]\d{0,2})(?<number>[0-9][\d]{7})$";
            string patternPhone = @"^(?<area>0?[1-9]\d{0,2})(?<number>[0-9][\d]{7})$";
            
            if (!String.IsNullOrEmpty(phoneNumber))
            {
                if (phoneNumber.Length > 9)
                {
                    if (Regex.IsMatch(phoneNumber, patternPhoneCountry))
                        return true;
                }
                else
                {
                    if (Regex.IsMatch(phoneNumber, patternPhone))
                        return true;
                }
            }

            return false;
        }
        
    }
}