using Bogus;
using Gafel.Domain.Dtos.QueryParams;
using Gafel.Domain.Enums;

namespace CommonTestUtilities.Dtos.QueryParams;

public class FilterBankAccountQueryParamsBuilder
{
    public static FilterBankAccountQueryParams Build(
        int? offset = null,
        int? limit = null,
        BankAccountType? type = null,
        bool nullOffset = false,
        bool nullLimit = false)
    {
        return new Faker<FilterBankAccountQueryParams>()
            .RuleFor(x => x.Offset, faker => nullOffset ? null : offset ?? faker.Random.Int(0, 100))
            .RuleFor(x => x.Limit, faker => nullLimit ? null : limit ?? faker.Random.Int(1, 100))
            .RuleFor(x => x.Type, faker => type ?? faker.PickRandom<BankAccountType>());
    }
}
