
using Microsoft.AspNetCore.Mvc;
using NievoEasyFin.Application.Interfaces.Request;

namespace NievoEasyFin.Application.Interfaces.Services
{
    /// <summary>
    /// Interface for accounts management services, handling bank accounts operations.
    /// </summary>
    public interface IAccountsService
    {
        /// <summary>
        /// Creates a new bank.
        /// </summary>
        /// <param name="request">The bank account creation request data.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the bank account creation.</returns>
        Task<IActionResult> PostAccountsBanks(PostAccountsBanksRequest request);

        /// <summary>
        /// Creates a new bank account for the authenticated user.
        /// </summary>
        /// <param name="request">PostUserBanksRequest</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the user bank account creation.</returns>
        Task<IActionResult> PostUserBanks(PostUserBanksRequest request);

        /// <summary>
        /// Get list of banks
        /// </summary>
        /// <param name="request">GetBanksRequest</param>
        Task<IActionResult> GetBanks(GetBanksRequest request);
    }
}
