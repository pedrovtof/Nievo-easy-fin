using System.ComponentModel;

namespace NievoEasyfin.Application.Interfaces.Enum
{
    /// <summary>
    /// Enum para armazenar os erros da API.
    /// ENDPOINT + SERVICE + STATUS_CODE + ERROR_MESSAGE
    /// </summary>
    public enum EnumErrosApi
    {
        /// <summary>
        /// Campo nome possuí valores invalidos.
        /// </summary>
        [Description("Campo nome possuí valores invalidos.")]
        POSTUSERASYNC_AUTHSERVICE_400_INVALID_NAME_INPUT,

        /// <summary>
        /// Campo senha possuí valores invalidos.
        /// </summary>
        [Description("Campo senha possuí valores invalidos.")]
        POSTUSERASYNC_AUTHSERVICE_400_INVALID_PASSWORD_INPUT,

        /// <summary>
        /// Campo email possuí valores invalidos.
        /// </summary>
        [Description("Campo email possuí valores invalidos.")]
        POSTUSERASYNC_AUTHSERVICE_400_INVALID_EMAIL_INPUT,

        /// <summary>
        /// Usuário já está cadastrado.
        /// </summary>
        [Description("Usuário já está cadastrado.")]
        POSTUSERASYNC_AUTHSERVICE_409_USER_ALREADY_EXISTS
    }
}