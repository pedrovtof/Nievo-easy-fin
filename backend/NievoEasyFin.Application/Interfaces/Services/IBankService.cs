namespace NievoEasyFin.Application.Interfaces.Services;

/// <summary>
/// Unified interface for bank management services, combining public and private bank operations.
/// </summary>
public interface IBankService : IPublicBankService, IPrivateBankService
{
}
