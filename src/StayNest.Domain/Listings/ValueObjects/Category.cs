using StayNest.SharedKernel.Domain;
using StayNest.SharedKernel.ErrorHandling;

namespace StayNest.Domain.Listings.ValueObjects;

public sealed class Category : ValueObject
{
    public const int MaxLength = 100;

    private Category(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<Category> Create(string category)
    {
        if (string.IsNullOrWhiteSpace(category))
        {
            return Result.Failure<Category>(new Error(
                "Category.Empty",
                "Category cannot be empty."));
        }

        if (category.Length > MaxLength)
        {
            return Result.Failure<Category>(new Error(
                "Category.TooLong",
                $"Category cannot exceed {MaxLength} characters."));
        }

        return Result.Success(new Category(category));
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
