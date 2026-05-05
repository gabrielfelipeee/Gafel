using Gafel.Application.Exceptions;
using Gafel.Domain.Repositories;
using Gafel.Domain.Repositories.Category;
using Gafel.Domain.Repositories.Person;
using Gafel.Domain.Resources;
using Gafel.Domain.Services.CurrentUser;

namespace Gafel.Application.UseCases.Category.Delete;

public class DeleteCategoryUseCase : IDeleteCategoryUseCase
{
    private readonly ICurrentUser _currentUser;
    private readonly IPersonReadOnlyRepository _personReadOnlyRepository;
    private readonly ICategoryReadOnlyRepository _categoryReadOnlyRepository;
    private readonly ICategoryWriteOnlyRepository _categoryWriteOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCategoryUseCase(
        ICurrentUser currentUser,
        IPersonReadOnlyRepository personReadOnlyRepository,
        ICategoryReadOnlyRepository categoryReadOnlyRepository,
        ICategoryWriteOnlyRepository categoryWriteOnlyRepository,
        IUnitOfWork unitOfWork)
    {
        _currentUser = currentUser;
        _personReadOnlyRepository = personReadOnlyRepository;
        _categoryReadOnlyRepository = categoryReadOnlyRepository;
        _categoryWriteOnlyRepository = categoryWriteOnlyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(long categoryId)
    {
        var currentUser = _currentUser.CurrentUser();
        var person = await _personReadOnlyRepository.GetByUserId(currentUser.Id) ?? throw new PersonNotFoundException();

        if (!await _categoryReadOnlyRepository.ExistActiveCategoryWithId(person, categoryId))
            throw new NotFoundException(ResourceMessagesException.CATEGORY_NOT_FOUND);

        await _categoryWriteOnlyRepository.Delete(categoryId);
        await _unitOfWork.SaveChangesAsync();
    }
}
