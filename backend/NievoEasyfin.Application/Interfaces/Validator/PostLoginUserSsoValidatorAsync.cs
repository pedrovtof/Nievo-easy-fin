using NievoEasyfin.Application.Interfaces.Enum;
using NievoEasyfin.Application.Interfaces.Request;
using FluentValidation;

namespace NievoEasyfin.Application.Interfaces.Validator;

/// <summary>
/// Class template for abstractorValidator in PostLoginUserSsoRequest
/// </summary>
public class PostLoginUserSsoValidatorAsync : AbstractValidator<PostLogiPostLoginUserSsoRequest>
{
    /// <summary>
    /// Constructor to validate resquest in PostLoginUserSso
    /// </summary>
    public PostLoginUserSsoValidatorAsync()
    {
        RuleFor(x => x.Provider)
            .NotEmpty()
                .WithErrorCode(EnumErrosApi.POSTLOGINUSERSSOASYNC_AUTHSERVICE_400_PROVIDER_NULL_OR_EMPTY.ToString());

        RuleFor(x => x.ProviderAccessToken)
            .NotEmpty()
                .WithErrorCode(EnumErrosApi.POSTLOGINUSERSSOASYNC_AUTHSERVICE_400_PROVIDER_ACCESS_TOKEN_ID_NULL_OR_EMPTY.ToString());
    }
}
