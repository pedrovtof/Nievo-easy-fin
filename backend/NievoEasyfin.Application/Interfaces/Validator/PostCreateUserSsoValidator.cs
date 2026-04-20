using NievoEasyfin.Application.Interfaces.Enum;
using NievoEasyfin.Application.Interfaces.Request;
using FluentValidation;

namespace NievoEasyfin.Application.Interfaces.Validator;

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
    }
}
