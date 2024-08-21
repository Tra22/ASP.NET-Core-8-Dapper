using FluentValidation.Results;

namespace DapperWebAPIProject.Validation.Result;
public class ValidationFailureList: ValidationFailure {
    public int index { get; set; }
    public ValidationFailureList(ValidationFailure validationFailure, int index) : base(validationFailure.PropertyName, validationFailure.ErrorMessage){
        this.index = index;
    }
}