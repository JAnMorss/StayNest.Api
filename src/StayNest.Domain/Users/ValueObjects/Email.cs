using StayNest.SharedKernel.Domain;
using StayNest.SharedKernel.ErrorHandling;
using System.Net.Mail;

namespace StayNest.Domain.Users.ValueObjects;

public sealed class Email : ValueObject
{
    private Email(string value)
    {
        Value = value;
    }

    public string Value { get;  }

    public static Result<Email> Create(string email)
    {
        if(string.IsNullOrWhiteSpace(email))
        {
            return Result.Failure<Email>(new Error(
                "Email.Empty",
                "Email cannot be empty."
            ));
        }

        try
        {
            var addr = new MailAddress(email.Trim());

            return Result.Success(new Email(addr.Address));
        }
        catch
        {
            return Result.Failure<Email>(new Error(
                "Email.Invalid",
                "Invalid email address format."
            ));
        }
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    public bool Equals(Email other)
        => other != null && Value == other.Value;
}
