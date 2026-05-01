using Gafel.Application.Exceptions;
using Gafel.Application.Extensions;
using Gafel.Application.UseCases.Category.Shared.Commands;
using Gafel.Application.UseCases.Category.Shared.Validators;
using Gafel.Domain.Repositories;
using Gafel.Domain.Repositories.Category;
using Gafel.Domain.Repositories.Person;
using Gafel.Domain.Services.CurrentUser;
using Mapster;

namespace Gafel.Application.UseCases.Category.Register;

public class RegisterCategoryUseCase : IRegisterCategoryUseCase
{
    private readonly ICurrentUser _currentUser;
    private readonly ICategoryWriteOnlyRepository _categoryWriteOnlyRepository;
    private readonly IPersonReadOnlyRepository _personReadOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterCategoryUseCase(
        ICurrentUser currentUser,
        ICategoryWriteOnlyRepository categoryWriteOnlyRepository,
        IPersonReadOnlyRepository personReadOnlyRepository,
        IUnitOfWork unitOfWork)
    {
        _currentUser = currentUser;
        _categoryWriteOnlyRepository = categoryWriteOnlyRepository;
        _personReadOnlyRepository = personReadOnlyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(CategoryCommand request)
    {
        Validate(request);

        var currentUser = _currentUser.CurrentUser();

        var person = await _personReadOnlyRepository.GetByUserId(currentUser.Id) ?? throw new PersonNotFoundException();

        var category = request.Adapt<Domain.Entities.Category>();
        category.PersonId = person.Id;
        category.CreatedAt = DateTime.UtcNow;

        await _categoryWriteOnlyRepository.Add(category);
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
