using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;
using StayNest.SharedKernel.Domain;
using StayNest.SharedKernel.ErrorHandling;
using System.Security.Cryptography;

namespace StayNest.Domain.Users.ValueObjects;

public sealed class HashedPassword : ValueObject
{
    private HashedPassword(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static HashedPassword FromPlainText(string password)
    {
        if (string.IsNullOrEmpty(password))
            throw new ArgumentException(
                "Password cannot be empty.", 
                nameof(password));

        byte[] salt = RandomNumberGenerator.GetBytes(16);
        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 10000,
            numBytesRequested: 32));

        return new HashedPassword($"{Convert.ToBase64String(salt)}.{hashed}");
    }

    public bool Verify(string password)
    {
        var parts = Value.Split('.');
        if (parts.Length != 2)
            return false;

        var salt = Convert.FromBase64String(parts[0]);
        var storedHash = parts[1];

        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 10000,
            numBytesRequested: 32));

        return storedHash == hashed;
    }

    public static Result<HashedPassword> Create(string hash)
    {
        if (string.IsNullOrWhiteSpace(hash))
        {
            return Result.Failure<HashedPassword>(new Error(
                "HashedPassword.Empty",
                "Password hash cannot be empty."));
        }

        if (hash.Length < 5)
        {
            return Result.Failure<HashedPassword>(new Error(
                "HashedPassword.Invalid",
                "Hashed password format is invalid or too short."));
        }

        return Result.Success(new HashedPassword(hash));
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
