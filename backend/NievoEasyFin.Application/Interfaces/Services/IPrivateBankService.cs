using Microsoft.AspNetCore.Mvc;
using NievoEasyFin.Application.Interfaces.Request;

namespace NievoEasyFin.Application.Interfaces.Services;

/// <summary>
/// Interface for private bank services, handling bank and bank card creation.
/// </summary>
public interface IPrivateBankService
{
    /// <summary>
    /// Creates a new bank.
    /// </summary>
    /// <param name="request">The bank account creation request data.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the bank account creation.</returns>
    Task<IActionResult> PostAccountsBanks(PostAccountsBanksRequest request);

    /// <summary>
    /// Create an bank card
    /// </summary>
    /// <param name="request">PostBankCardRequest</param>
    Task<IActionResult> PostBankCard(PostBankCardRequest request);
}
