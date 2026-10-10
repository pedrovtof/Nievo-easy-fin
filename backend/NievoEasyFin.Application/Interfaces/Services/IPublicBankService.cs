using Microsoft.AspNetCore.Mvc;
using NievoEasyFin.Application.Interfaces.Request;

namespace NievoEasyFin.Application.Interfaces.Services;

/// <summary>
/// Interface for public bank services, handling bank and bank cards queries.
/// </summary>
public interface IPublicBankService
{
    /// <summary>
    /// Get list of banks
    /// </summary>
    /// <param name="request">GetBanksRequest</param>
    Task<IActionResult> GetBanks(GetBanksRequest request);

    /// <summary>
    /// Get list of card registred of the bank
    /// </summary>
    /// <param name="request">GetBankCardRequest</param>
    Task<IActionResult> GetBankCard(GetBankCardRequest request);
}
