using Gafel.Domain.Common;
using Gafel.Domain.Resources;

namespace Gafel.Domain.ValueObjects;

public sealed partial record DateOfBirth
{
    private const int MINIMUM_AGE = 18;

    public DateOnly Value { get; }
    private DateOfBirth(DateOnly value) => Value = value;

    public static Result<DateOfBirth> Create(DateOnly date)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        if (date > today)
            return Result<DateOfBirth>.Failure(errorMessage: ResourceMessagesException.PERSON_DATE_OF_BIRTH_FUTURE);

        if (date > today.AddYears(-MINIMUM_AGE))
            return Result<DateOfBirth>.Failure(errorMessage: ResourceMessagesException.PERSON_DATE_OF_BIRTH_INVALID);

        return Result<DateOfBirth>.Success(value: new DateOfBirth(date));
    }

    public override string ToString() => Value.ToString("yyyy-MM-dd");
}
