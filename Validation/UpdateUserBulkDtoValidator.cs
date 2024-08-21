using DapperWebAPIProject.Dto.Request;
using FluentValidation;

namespace DapperWebAPIProject.Validation;
public class UpdateUserBulkDtoValidator : AbstractValidator<UpdateUserBulkRequestDto>
{
    public UpdateUserBulkDtoValidator()
    {
        RuleFor(x => x.Id).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Name).NotEmpty().Length(1, 255);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
    }
}