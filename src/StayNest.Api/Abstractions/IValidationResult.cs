using StayNest.SharedKernel.ErrorHandling;

namespace StayNest.Api.Abstractions;

public interface IValidationResult
{
    public static readonly Error ValidationError = new(
        "ValidationError",
        "A validation problem occurred.");

    Error[] Errors { get; }
}