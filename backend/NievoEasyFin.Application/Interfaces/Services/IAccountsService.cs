namespace NievoEasyFin.Application.Interfaces.Services;

/// <summary>
/// Interface for accounts management services, aggregating card, bank, and user services.
/// </summary>
public interface IAccountsService : ICardService, IBankService, IUserService
{
}
