using Gafel.Domain.Constants;
using Gafel.Domain.Dtos.QueryParams;
using Gafel.Domain.Enums;

namespace CommonTestUtilities.Dtos.QueryParams;

public class FilterTransactionQueryParamsBuilder
{
    public static FilterTransactionQueryParams Build()
    {
        return FakerFactory.Create<FilterTransactionQueryParams>()
            .RuleFor(x => x.Type, faker => faker.PickRandom<CategoryType>())
            .RuleFor(x => x.IsSettled, faker => faker.Random.Bool())
            .RuleFor(x => x.MinimumAmount, faker => faker.Finance.Amount(min: DomainRules.MinimumTransactionAmount, max: DomainRules.MaximumMoneyAmount))
            .RuleFor(x => x.MaximumAmount, (faker, query) => faker.Finance.Amount(min: query.MinimumAmount!.Value, max: DomainRules.MaximumMoneyAmount));
    }
}
