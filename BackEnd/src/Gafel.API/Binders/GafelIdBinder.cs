using Microsoft.AspNetCore.Mvc.ModelBinding;
using Gafel.Application.Exceptions;
using Sqids;
using Gafel.Domain.Resources;

namespace Gafel.API.Binders;
public class GafelIdBinder(SqidsEncoder<long> sqids) : IModelBinder
{
    private readonly SqidsEncoder<long> _sqids = sqids;

    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var modelName = bindingContext.ModelName;

        var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);

        if (valueProviderResult == ValueProviderResult.None)
            return Task.CompletedTask;

        var value = valueProviderResult.FirstValue;

        if (string.IsNullOrWhiteSpace(value))
            ThrowValidationException(modelName);

        var decoded = _sqids.Decode(value).ToArray();

        if (decoded.Length != 1)
            ThrowValidationException(modelName);

        bindingContext.Result = ModelBindingResult.Success(decoded[0]);

        return Task.CompletedTask;
    }

    private static void ThrowValidationException(string modelName)
    {
        throw new ErrorOnValidationException(
            new Dictionary<string, string[]>
            {
                [modelName] = [ResourceMessagesException.INVALID_IDENTIFIER]
            });
    }
}
