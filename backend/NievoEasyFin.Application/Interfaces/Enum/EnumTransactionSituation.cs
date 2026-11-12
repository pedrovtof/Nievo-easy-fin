using System.ComponentModel;

namespace NievoEasyFin.Application.Interfaces.Enum
{
    /// <summary>
    /// Enum representing the financial transaction cash-flow situation.
    /// </summary>
    public enum EnumTransactionSituation
    {
        /// <summary>
        /// Inflow transaction (Entrada).
        /// </summary>
        [Description("Entrada")]
        ENTRADA = 1,

        /// <summary>
        /// Outflow transaction (Saida).
        /// </summary>
        [Description("Saida")]
        SAIDA = 2
    }
}