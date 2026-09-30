using FluentValidation;
using NievoEasyFin.Application.Interfaces.Enum;
using NievoEasyFin.Application.Interfaces.Request;

namespace NievoEasyFin.Application.Interfaces.Validator
{
    public class PostUserCategoryValidatorAsync : AbstractValidator<PostUserCategoryRequest>
    {
        public PostUserCategoryValidatorAsync()
        {
            RuleFor(x => x.GetEmail())
                .NotEmpty()
                    .WithErrorCode(EnumErrosApi.POSTUSERCATEGORYASYNC_CORESERVICE_400_EMPTY_EMAIL.ToString())
                .EmailAddress()
                    .WithErrorCode(EnumErrosApi.POSTUSERGOALASYNC_CORESERVICE_400_INVALID_EMAIL.ToString());

            RuleFor(x => x.Name)
                .NotEmpty()
                    .WithErrorCode(EnumErrosApi.POSTUSERCATEGORYASYNC_CORESERVICE_400_EMPTY_NAME.ToString())
                .Must(x => x.Length > 1 && x.Length < 100)
                    .WithErrorCode(EnumErrosApi.POSTUSERCATEGORYASYNC_CORESERVICE_400_INVALID_NAME.ToString());

            RuleFor(x => x.Goal)
                .GreaterThan(0)
                .WithErrorCode(EnumErrosApi.POSTUSERCATEGORYASYNC_CORESERVICE_400_INVALID_GOAL.ToString());

            RuleFor(x => x.ParentCategory)
                .GreaterThan(0)
                .When(x => x.ParentCategory != null)
                .WithErrorCode(EnumErrosApi.POSTUSERCATEGORYASYNC_CORESERVICE_400_INVALID_PARENTCATEGORY.ToString());
        }
    }
}
