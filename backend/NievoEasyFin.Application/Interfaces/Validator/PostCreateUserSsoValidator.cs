using NievoEasyFin.Application.Interfaces.Enum;
using NievoEasyFin.Application.Interfaces.Request;
using FluentValidation;
using NievoEasyFin.Application.Extensions.Enum;

namespace NievoEasyFin.Application.Interfaces.Validator;

/// <summary>
/// Class template for abstractorValidator in PostCreateUserSso
/// </summary>
public class PostCreateUserSsoValidator : AbstractValidator<PostCreateUserSsoRequest>
{
    /// <summary>
    /// Constructor to validate resquest in PostCreateUserSso
    /// </summary>
    public PostCreateUserSsoValidator()
    {
        RuleFor(x => x.Provider)
            .NotEmpty()
                .WithErrorCode(EnumErrosApi.POSTCREATEUSERSSOASYNC_AUTHSERVICE_400_PROVIDER_NULL_OR_EMPTY.ToString());

        RuleFor(x => x.ProviderAccessToken)
            .NotEmpty()
                .WithErrorCode(EnumErrosApi.POSTCREATEUSERSSOASYNC_AUTHSERVICE_400_PROVIDER_ACCESS_TOKEN_ID_NULL_OR_EMPTY.ToString());

        RuleFor(x => x.GetHost())
            .NotEmpty()
                .WithErrorCode(EnumErrosApi.POSTCREATEUSERSSOASYNC_AUTHSERVICE_400_HOST_NULL_OR_EMPTY.ToString());

        RuleFor(x => x.GetUserAgent())
            .NotEmpty()
                .WithErrorCode(EnumErrosApi.POSTCREATEUSERSSOASYNC_AUTHSERVICE_400_USER_AGENT_NULL_OR_EMPTY.ToString());

        RuleFor(x => x.AcceptTerms)
            .Must(x => x == true)
                .WithErrorCode(EnumErrosApi.POSTCREATEUSERSSOASYNC_AUTHSERVICE_400_TERMS_NOT_ACCEPTED.GetDescription());
    }
}
