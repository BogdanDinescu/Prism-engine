using System;
using Prism.Data;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using DnsClient;

namespace Prism.Models
{
    public class EmailValidator : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var user = (User)validationContext.ObjectInstance;
            string email = user.Email;
            if (email == null)
                return new ValidationResult("Email is null");

            Regex rx = new Regex(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}",RegexOptions.IgnoreCase);
            MatchCollection matches = rx.Matches(email);

            if (matches.Count == 0)
                return new ValidationResult("Email is incorrect!");

            var context = (DatabaseCtx)validationContext.GetService(typeof(DatabaseCtx));
            var entity = context.Users.SingleOrDefault(e => e.Email == email);

            if (entity != null)
                return new ValidationResult("Email is already in use");

            var options = new LookupClientOptions();
            options.Timeout = TimeSpan.FromSeconds(5);
            var lookup = new LookupClient(options);
            var result = lookup.Query(email.Split('@')[1], QueryType.MX);
            var records = result.Answers;

            if (!records.Any())
                return new ValidationResult("Email does not exist");

            return ValidationResult.Success;
        }
    }
}