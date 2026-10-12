namespace NievoEasyFin.Application.Interfaces.Services;

/// <summary>
/// Interface for accounts management services, aggregating card, bank, payment and user services.
/// </summary>
public interface IAccountsService : ICardService, IBankService, IUserService, IPaymentService
{
}
