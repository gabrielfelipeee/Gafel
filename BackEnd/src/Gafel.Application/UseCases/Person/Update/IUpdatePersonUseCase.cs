namespace Gafel.Application.UseCases.Person.Update;

public interface IUpdatePersonUseCase
{
    Task Execute(UpdatePersonCommand request);
}
