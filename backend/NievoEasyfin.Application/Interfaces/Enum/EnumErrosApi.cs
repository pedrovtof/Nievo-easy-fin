using System.ComponentModel;

namespace NievoEasyfin.Application.Interfaces.Enum
{
    /// <summary>
    /// Enum for API erros.
    /// The message by default is PT-BR.
    /// ENDPOINT + SERVICE + STATUS_CODE + ERROR_MESSAGE
    /// </summary>
    public enum EnumErrosApi
    {

        #region #PostCreateUserAsync

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
        POSTCREATEUSERASYNC_AUTHSERVICE_409_USER_ALREADY_EXISTS,

        #endregion PostCreateUserAsync

        #region #PostCreateUserSsoAsync

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
        /// Provider name is null or empty
        /// </summary>
        POSTCREATEUSERSSOASYNC_AUTHSERVICE_400_PROVIDER_ACCESS_TOKEN_ID_NULL_OR_EMPTY,


        /// <summary>
        /// Provider id is not valid
        /// </summary>
        POSTCREATEUSERSSOASYNC_AUTHSERVICE_400_PROVIDER_ACCESS_TOKEN_ID_INVALID

        #endregion #PostCreateUserSsoAsync
    }
}