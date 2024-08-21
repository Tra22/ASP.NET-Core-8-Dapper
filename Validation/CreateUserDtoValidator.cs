using DapperWebAPIProject.Dto.Request;
using FluentValidation;

namespace DapperWebAPIProject.Validation;
public class CreateUserDtoValidator : AbstractValidator<CreateUserRequestDto>
{
    public CreateUserDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().Length(1, 255);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
    }
}