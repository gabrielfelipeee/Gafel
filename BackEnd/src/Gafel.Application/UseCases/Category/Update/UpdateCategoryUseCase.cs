using Gafel.Application.Exceptions;
using Gafel.Application.Extensions;
using Gafel.Application.UseCases.Category.Shared.Commands;
using Gafel.Application.UseCases.Category.Shared.Validators;
using Gafel.Domain.Repositories;
using Gafel.Domain.Repositories.Category;
using Gafel.Domain.Repositories.Person;
using Gafel.Domain.Resources;
using Gafel.Domain.Services.CurrentUser;
using Mapster;

namespace Gafel.Application.UseCases.Category.Update;

public class UpdateCategoryUseCase : IUpdateCategoryUseCase
{
    private readonly ICurrentUser _currentUser;
    private readonly ICategoryUpdateOnlyRepository _categoryUpdateOnlyRepository;
    private readonly IPersonReadOnlyRepository _personReadOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCategoryUseCase(
        ICurrentUser currentUser,
        ICategoryUpdateOnlyRepository categoryUpdateOnlyRepository,
        IPersonReadOnlyRepository personReadOnlyRepository,
        IUnitOfWork unitOfWork)
    {
        _currentUser = currentUser;
        _categoryUpdateOnlyRepository = categoryUpdateOnlyRepository;
        _personReadOnlyRepository = personReadOnlyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(long categoryId, CategoryCommand request)
    {
        Validate(request);

        var currentUser = _currentUser.CurrentUser();
        var person = await _personReadOnlyRepository.GetByUserId(currentUser.Id) ?? throw new PersonNotFoundException();

        var category = await _categoryUpdateOnlyRepository.GetById(person, categoryId) ?? throw new NotFoundException(ResourceMessagesException.CATEGORY_NOT_FOUND);
        request.Adapt(category);
        category.UpdatedAt = DateTime.UtcNow;

        _categoryUpdateOnlyRepository.Update(category);
        await _unitOfWork.SaveChangesAsync();
    }

    private static void Validate(CategoryCommand request)
    {
        var result = new CategoryValidator().Validate(request);

        if (!result.IsValid)
        {
            var errors = result.Errors.ToErrorsByPropertyDictionary();

            throw new ErrorOnValidationException(errors);
        }
    }
}
