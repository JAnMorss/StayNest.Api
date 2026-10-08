using StayNest.SharedKernel.Domain;
using StayNest.SharedKernel.ErrorHandling;

namespace StayNest.Domain.Shared.ValueObjects;

public sealed class DateRange : ValueObject
{
    private DateRange(
        DateTime startDate,
        DateTime endDate)
    {
        StartDate = startDate;
        EndDate = endDate;
    }

    public DateTime StartDate { get; }

    public DateTime EndDate { get; }

    public int LengthInDays =>
        (EndDate - StartDate).Days;

    public bool Overlaps(DateRange other)
    {
        return StartDate < other.EndDate && EndDate > other.StartDate;
    }

    public static Result<DateRange> Create(
        DateTime startDate,
        DateTime endDate
    )
    {
        startDate = startDate.Date;
        endDate = endDate.Date;

        if (startDate >= endDate)
        {
            return Result.Failure<DateRange>(new Error(
                "DateRange.Invalid",
                "Start date must be before end date."));
        }

        return Result.Success(new DateRange(startDate, endDate));
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return StartDate;
        yield return EndDate;
    }
}