using Microsoft.AspNetCore.Mvc;
using NievoEasyFin.Application.Interfaces.Request;

namespace NievoEasyFin.Application.Interfaces.Services;

/// <summary>
/// Interface for card services, handling card types and card flags operations.
/// </summary>
public interface ICardService
{
    /// <summary>
    /// Get list of card types
    /// </summary>
    /// <param name="request">GetCardTypeRequest</param>
    Task<IActionResult> GetCardType(GetCardTypeRequest request);

    /// <summary>
    /// Get list of card flag
    /// </summary>
    /// <param name="request">GetCardFlagRequest</param>
    /// <returns></returns>
    Task<IActionResult> GetCardFlag(GetCardFlagRequest request);
}
