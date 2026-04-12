using System.ComponentModel;

namespace NievoEasyfin.Application.Interfaces.Enum
{
    /// <summary>
    /// Enum for API erros.
    /// The message by default is ENGLISH-EUA.
    /// ENDPOINT + SERVICE + STATUS_CODE + ERROR_MESSAGE
    /// </summary>
    public enum EnumErrosApi
    {
        #region CreateUserAsync

        /// <summary>
        /// Name must not be empty
        /// </summary>
        POSTCREATEUSERASYNC_AUTHSERVICE_400_NAME_EMPTY_NULL,

        /// <summary>
        /// The name must be between 2 and 100 letters
        /// </summary>
        POSTCREATEUSERASYNC_AUTHSERVICE_400_NAME_WITH_WRONG_LENGHT,

        /// <summary>
        /// Password must not be empty
        /// </summary>
        POSTCREATEUSERASYNC_AUTHSERVICE_400_PASSWORD_EMPTY_NULL,

        /// <summary>
        /// The password must be between 6 and 12 letters
        /// </summary>
        POSTCREATEUSERASYNC_AUTHSERVICE_400_PASSWORD_WITH_WRONG_LENGHT,

        /// <summary>
        /// Password must have at least one uppercase letter, one lowercase letter, one number and one special character
        /// </summary>
        POSTCREATEUSERASYNC_AUTHSERVICE_400_PASSWORD_WRONG_FORMAT,

        /// <summary>
        /// Email must not be empty
        /// </summary>
        POSTCREATEUSERASYNC_AUTHSERVICE_400_EMAIL_EMPTY_NULL,

        /// <summary>
        /// Email already exists
        /// </summary>
        [Description("Email already exists in the system")]
        POSTCREATEUSERASYNC_AUTHSERVICE_400_EMAIL_ALREADY_EXISTS,

        /// <summary>
        /// Email has invalid value
        /// </summary>
        POSTCREATEUSERASYNC_AUTHSERVICE_400_EMAIL_INVALID,

        /// <summary>
        /// User already exist
        /// </summary>
        [Description("User created with sucess")]
        POSTCREATEUSERASYNC_AUTHSERVICE_201_CREATED,

        #endregion CreateUserAsync

        #region CreateUserSsoAsync

        /// <summary>
        /// Provider name is null or empty
        /// </summary>
        POSTCREATEUSERSSOASYNC_AUTHSERVICE_400_PROVIDER_NULL_OR_EMPTY,

        /// <summary>
        /// Provider do not exists
        /// </summary>
        [Description("This provider is unknown")]
        POSTCREATEUSERSSOASYNC_AUTHSERVICE_400_PROVIDER_NOT_CONFIGURED,

        /// <summary>
        /// Provider do not exists
        /// </summary>
        [Description("Invalid provider response")]
        POSTCREATEUSERSSOASYNC_AUTHSERVICE_400_PROVIDER_NOT_200_RESPONSE,

        /// <summary>
        /// Provider name is null or empty
        /// </summary>
        POSTCREATEUSERSSOASYNC_AUTHSERVICE_400_PROVIDER_ACCESS_TOKEN_ID_NULL_OR_EMPTY,

        /// <summary>
        /// Provider id is not valid
        /// </summary>
        POSTCREATEUSERSSOASYNC_AUTHSERVICE_400_PROVIDER_ACCESS_TOKEN_ID_INVALID,

        /// <summary>
        /// User already exist
        /// </summary>
        [Description("User alerady exists")]
        POSTCREATEUSERSSOASYNC_AUTHSERVICE_200_USER_ALREADY_EXISTS,

        /// <summary>
        /// User already exist
        /// </summary>
        [Description("User created with sucess")]
        POSTCREATEUSERSSOASYNC_AUTHSERVICE_201_CREATED,

        #endregion CreateUserSsoAsync

        #region LoginUserRequest

        /// <summary>
        /// Email is empty or null
        /// </summary>
        POSTLOGINUSERASYNC_AUTHSERVICE_400_EMAIL_EMPTY_NULL,

        /// <summary>
        /// Password is empty or null
        /// </summary>
        POSTLOGINUSERASYNC_AUTHSERVICE_400_PASSWORD_EMPTY_NULL,

        /// <summary>
        /// User not found
        /// </summary>
        [Description("The user may not have an account or the password is incorrect")]
        POSTLOGINUSERASYNC_AUTHSERVICE_404_USER_NOT_FOUND,

        /// <summary>
        /// User not found
        /// </summary>
        [Description("The user may not have an account or the password is incorrect")]
        POSTLOGINUSERASYNC_AUTHSERVICE_400_WRONG_PASSWORD,

        /// <summary>
        /// Provider Sso do not exists
        /// </summary>
        [Description("The user may not have an account or the password is incorrect")]
        POSTLOGINUSERSSOASYNC_AUTHSERVICE_400_PROVIDERSSO_NOT_CONFIGURED,

        /// <summary>
        /// Provider do not exists
        /// </summary>
        [Description("This provider is unknown")]
        POSTLOGINUSERSSOASYNC_AUTHSERVICE_400_PROVIDER_NOT_CONFIGURED,

        /// <summary>
        /// Provider do not exists
        /// </summary>
        [Description("Invalid provider response")]
        POSTLOGINUSERSSOASYNC_AUTHSERVICE_400_PROVIDER_NOT_200_RESPONSE,

        /// <summary>
        /// Provider name is null or empty
        /// </summary>
        POSTLOGINUSERSSOASYNC_AUTHSERVICE_400_PROVIDER_NULL_OR_EMPTY,

        /// <summary>
        /// Provider name is null or empty
        /// </summary>
        POSTLOGINUSERSSOASYNC_AUTHSERVICE_400_PROVIDER_ACCESS_TOKEN_ID_NULL_OR_EMPTY,

        #endregion LoginUserRequest
    }
}
