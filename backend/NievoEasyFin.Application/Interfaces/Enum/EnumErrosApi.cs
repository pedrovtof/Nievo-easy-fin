using System.ComponentModel;

namespace NievoEasyFin.Application.Interfaces.Enum;

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
    [Description("User already exists or email is invalid")]
    POSTCREATEUSERASYNC_AUTHSERVICE_400_EMAIL_ALREADY_EXISTS,

    /// <summary>
    /// Email has invalid value
    /// </summary>
    POSTCREATEUSERASYNC_AUTHSERVICE_400_EMAIL_INVALID,

    /// <summary>
    /// The user most provider the host
    /// </summary>
    POSTCREATEUSERASYNC_AUTHSERVICE_400_HOST_NULL_OR_EMPTY,

    /// <summary>
    /// The user most provider the AGENT
    /// </summary>
    POSTCREATEUSERASYNC_AUTHSERVICE_400_USER_AGENT_NULL_OR_EMPTY,

    /// <summary>
    /// The user most accept the terms
    /// </summary>
    POSTCREATEUSERASYNC_AUTHSERVICE_400_TERMS_NOT_ACCEPTED,

    /// <summary>
    /// Unfortunately an error happened during the process confirming the terms, please try again later
    /// </summary>
    [Description("Unfortunately an error happened during the process confirming the terms, please try again later")]
    POSTCREATEUSERASYNC_AUTHSERVICE_400_ERROR_WHILE_ACCEPT_TERMS,

    /// <summary>
    /// User not exists, user is blocked or email is not validated
    /// </summary>
    [Description("User not exists, user is blocked or email is not validated")]
    POSTCREATEUSERASYNC_AUTHSERVICE_400_EMAIL_NOT_VALIDATED,

    /// <summary>
    /// User created with sucess"
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
    /// Provider inactive
    /// </summary>
    [Description("This provider may be inactive")]
    POSTCREATEUSERSSOASYNC_AUTHSERVICE_400_PROVIDER_INACTIVE,

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
    /// The user most provider the host
    /// </summary>
    POSTCREATEUSERSSOASYNC_AUTHSERVICE_400_HOST_NULL_OR_EMPTY,

    /// <summary>
    /// The user most provider the AGENT
    /// </summary>
    POSTCREATEUSERSSOASYNC_AUTHSERVICE_400_USER_AGENT_NULL_OR_EMPTY,

    /// <summary>
    /// The user most accept the terms
    /// </summary>
    [Description("The user most accept the terms")]
    POSTCREATEUSERSSOASYNC_AUTHSERVICE_400_TERMS_NOT_ACCEPTED,

    /// <summary>
    /// Unfortunately an error happened during the process confirming the terms, please try again later
    /// </summary>
    [Description("Unfortunately an error happened during the process confirming the terms, please try again later")]
    POSTCREATEUSERSSOASYNC_AUTHSERVICE_400_ERROR_WHILE_ACCEPT_TERMS,

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
    /// The user may not have an account or the password is incorrect
    /// </summary>
    [Description("The user may not have an account or the password is incorrect")]
    POSTLOGINUSERASYNC_AUTHSERVICE_404_USER_NOT_FOUND,

    /// <summary>
    /// The user may be blocked or invalidated
    /// </summary>
    [Description("The user may be blocked or invalidated")]
    POSTLOGINUSERASYNC_AUTHSERVICE_404_USER_BLOCKED,

    /// <summary>
    /// User with wrong password,
    /// </summary>
    [Description("The user may not have an account or the password is incorrect")]
    POSTLOGINUSERASYNC_AUTHSERVICE_400_WRONG_PASSWORD,

    /// <summary>
    /// Provider Sso do not exists
    /// </summary>
    [Description("The user may not have an account or the password is incorrect")]
    POSTLOGINUSERSSOASYNC_AUTHSERVICE_400_PROVIDERSSO_NOT_CONFIGURED,

    /// <summary>
    /// The user may not have an account
    /// </summary>
    [Description("The user may not have an account")]
    POSTLOGINUSERSSOASYNC_AUTHSERVICE_400_USER_BLOCKED,

    /// <summary>
    /// The user may is blocked or the password is incorrect
    /// </summary>
    [Description("The user may is blocked or the password is incorrect")]
    POSTLOGINUSERSSOASYNC_AUTHSERVICE_404_USER_NOT_FOUND,

    /// <summary>
    /// Provider do not exists
    /// </summary>
    [Description("This provider is unknown")]
    POSTLOGINUSERSSOASYNC_AUTHSERVICE_400_PROVIDER_NOT_CONFIGURED,

    /// <summary>
    /// Provider inactive
    /// </summary>
    [Description("This provider may be inactive")]
    POSTLOGINUSERSSOASYNC_AUTHSERVICE_400_PROVIDER_INACTIVE,

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

    #region ResetPasswordRequest

    /// <summary>
    /// Invalid email
    /// </summary>
    POSTRESETPASSWORDASYNC_AUTHSERVICE_400_INVALID_EMAIL,

    /// <summary>
    /// Empty or null email
    /// </summary>
    POSTRESETPASSWORDASYNC_AUTHSERVICE_400_EMAIL_NULL_OR_EMPTY,

    /// <summary>
    /// The user may not have an account or may need to wait a bit longer for another token.
    /// </summary>
    [Description("The user may not have an account or may need to wait a bit longer for another token.")]
    POSTRESETPASSWORDASYNC_AUTHSERVICE_404_USER_NOT_FOUNND,

    [Description("The user may not have an account or may need to wait a bit longer for another token.")]
    POSTRESETPASSWORDASYNC_AUTHSERVICE_400_USER_TOKEN_FOUND_IN_CACHE,

    [Description("Token created.")]
    POSTRESETPASSWORDASYNC_AUTHSERVICE_201_USER_TOKEN,

    /// <summary>
    /// Invalid email
    /// </summary>
    PATCHRESETPASSWORDASYNC_AUTHSERVICE_400_INVALID_EMAIL,

    /// <summary>
    /// Empty or null email
    /// </summary>
    PATCHRESETPASSWORDASYNC_AUTHSERVICE_400_EMAIL_NULL_OR_EMPTY,

    /// <summary>
    /// Email not found, validate if you typed it correctly
    /// </summary>
    [Description("Email not found, validate if you typed it correctly")]
    PATCHRESETPASSWORDASYNC_AUTHSERVICE_404_USER_NOT_FOUND,

    /// <summary>
    /// Invalid Token format, must be bigger than 0
    /// </summary>
    [Description("Invalid Token format, must be bigger than 0")]
    PATCHRESETPASSWORDASYNC_AUTHSERVICE_400_INVALID_TOKEN_FORMAT,

    /// <summary>
    /// The user may not have an account or may need to request another token.
    /// </summary>
    [Description("The user may not have an account or may need to request another token.")]
    PATCHRESETPASSWORDASYNC_AUTHSERVICE_404_USER_TOKEN_NOT_FOUND_IN_CACHE,

    /// <summary>
    /// 
    /// </summary>
    [Description("The token is not valid")]
    PATCHRESETPASSWORDASYNC_AUTHSERVICE_400_TOKEN_INVALID,

    /// <summary>
    /// Password reset with sucess
    /// </summary>
    [Description("The token is not valid")]
    PATCHRESETPASSWORDASYNC_AUTHSERVICE_200_PASSWORD_RESET,

    /// <summary>
    /// Password must not be empty
    /// </summary>
    PATCHRESETPASSWORDASYNC_AUTHSERVICE_400_PASSWORD_EMPTY_NULL,

    /// <summary>
    /// The password must be between 6 and 12 letters
    /// </summary>
    [Description("The token is not valid")]
    PATCHRESETPASSWORDASYNC_AUTHSERVICE_400_PASSWORD_WITH_WRONG_LENGHT,

    /// <summary>
    /// Password must have at least one uppercase letter, one lowercase letter, one number and one special character
    /// </summary>
    PATCHRESETPASSWORDASYNC_AUTHSERVICE_400_PASSWORD_WRONG_FORMAT,

    /// <summary>
    /// The new password must not be the same as the current one.
    /// </summary>
    [Description("The new password must not be the same as the current one")]
    PATCHRESETPASSWORDASYNC_AUTHSERVICE_400_PASSWORD_IS_THE_SAME,

    /// <summary>
    /// A error was ocurred, please try again latter.
    /// </summary>
    [Description("An error has happened during the operation. Please try again later.")]
    PATCHRESETPASSWORDASYNC_AUTHSERVICE_400_PASSWORD_NOT_UPDATED,

    /// <summary>
    /// Password changed.
    /// </summary>
    [Description("Password changed.")]
    PATCHRESETPASSWORDASYNC_AUTHSERVICE_200_PASSWORD_CHANGED,

    #endregion ResetPasswordRequest

    #region ValidateEmailRequest

    POSTVALIDATEEMAILASYNC_AUTHSERVICE_400_EMPTY_EMAIL,

    POSTVALIDATEEMAILASYNC_AUTHSERVICE_400_INVALID_EMAIL,

    /// <summary>
    /// Token is not valid
    /// </summary>
    [Description("The token is not valid")]
    POSTVALIDATEEMAILASYNC_AUTHSERVICE_400_INVALID_TOKEN,

    /// <summary>
    /// The user may not have an account, email is incorrect or The user may have already been validated
    /// </summary>
    [Description("The user may not have an account, email is incorrect or The user may have already been validated")]
    POSTVALIDATEEMAILASYNC_AUTHSERVICE_404_USER_NOT_FOUND,

    /// <summary>
    /// The user may have already been validated
    /// </summary>
    [Description("The user may not have an account, email is incorrect or The user may have already been validated")]
    POSTVALIDATEEMAILASYNC_AUTHSERVICE_404_USER_BLOCKED_OR_VALIDATED,

    /// <summary>
    /// Was not possible to found one token for this email
    /// </summary>
    [Description("Was not possible to found one token for this email")]
    POSTVALIDATEEMAILASYNC_AUTHSERVICE_404_TOKEN_NOTFOUND_IN_CACHE,

    /// <summary>
    /// Token does not match, please try again
    /// </summary>
    [Description("Token does not match, please try again")]
    POSTVALIDATEEMAILASYNC_AUTHSERVICE_404_WRONG_TOKEN,

    /// <summary>
    /// Email validated.
    /// </summary>
    [Description("Email validated")]
    POSTVALIDATEEMAILASYNC_AUTHSERVICE_200_USER_VALIDATED,

    /// <summary>
    /// There has an error during the process, pleasy try again latter
    /// </summary>
    [Description("There has an error during the process, pleasy try again latter")]
    POSTVALIDATEEMAILASYNC_AUTHSERVICE_200_ERROR_VALIDATE_EMAIL,

    #endregion ValidateEmailRequest

    #region SendValidateEmailRequest

    POSTVALIDATEEMAILSENDASYNC_AUTHSERVICE_400_EMPTY_EMAIL,

    POSTVALIDATEEMAILSENDASYNC_AUTHSERVICE_400_INVALID_EMAIL,

    /// <summary>
    /// "The user may have already been validated, not have an account or email is incorrect"
    /// </summary>
    [Description("The user may have already been validated, not have an account or email is incorrect")]
    POSTVALIDATEEMAILSENDASYNC_AUTHSERVICE_404_USER_NOT_FOUND,

    /// <summary>
    /// "The user may have already been validated, not have an account or email is incorrect"
    /// </summary>
    [Description("The user may have already been validated, not have an account or email is incorrect")]
    POSTVALIDATEEMAILSENDASYNC_AUTHSERVICE_404_USER_BLOCKED_OR_VALIDATED,

    /// <summary>
    /// Token already exists, please wait and try again later
    /// </summary>
    [Description("Token already exists, please wait and try again later")]
    POSTVALIDATEEMAILSENDASYNC_AUTHSERVICE_400_TOKEN_FOUND_IN_CACHE,

    /// <summary>
    /// Token created.
    /// </summary>
    [Description("Token created.")]
    POSTVALIDATEEMAILSENDASYNC_AUTHSERVICE_200_TOKEN_CREATED,

    #endregion SendValidateEmailRequest

    #region GetAcceptTerms

    /// <summary>
    /// Terms not found
    /// </summary>
    [Description("Terms not found")]
    GETACCEPTTERMSASYNC_AUTHSERVICE_400_TERMS_NOT_FOUND,

    #endregion GetAcceptTerms

    #region GetAcceptTerms

    /// <summary>
    /// Empty name.
    /// </summary>
    POSTACCOUNTSBANKS_CORESERVICE_400_EMPTY_NAME,

    /// <summary>
    /// Empty bankType.
    /// </summary>
    POSTACCOUNTSBANKS_CORESERVICE_400_EMPTY_BANKTYPE,

    /// <summary>
    /// Bank type less than 0.
    /// </summary>
    POSTACCOUNTSBANKS_CORESERVICE_400_LESS_THAN_0_BANKTYPE,

    /// <summary>
    /// Empty userMail.
    /// </summary>
    [Description("Invalid user email")]
    POSTACCOUNTSBANKS_CORESERVICE_400_EMPTY_USERMAIL,

    /// <summary>
    /// Created with sucess
    /// </summary>
    [Description("Created with sucess")]
    POSTACCOUNTSBANKS_CORESERVICE_200_CREATED

    #endregion
}
