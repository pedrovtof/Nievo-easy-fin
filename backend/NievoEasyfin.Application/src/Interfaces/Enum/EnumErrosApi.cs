using System.ComponentModel;

namespace NievoEasyfin.Application.Interfaces.Enum
{
    public enum EnumErrosApi
    {
        /// <summary>
        /// Campo nome possuí valores invalidos.
        /// </summary>
        [Description("Campo nome possuí valores invalidos.")]
        INVALID_NAME_INPUT,

        /// <summary>
        /// Campo senha possuí valores invalidos.
        /// </summary>
        [Description("Campo senha possuí valores invalidos.")]
        INVALID_PASSWORD_INPUT,

        /// <summary>
        /// Campo email possuí valores invalidos.
        /// </summary>
        [Description("Campo email possuí valores invalidos.")]
        INVALID_EMAIL_INPUT


    }
}