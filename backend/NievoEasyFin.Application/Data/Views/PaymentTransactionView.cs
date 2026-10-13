using NievoEasyFin.Application.Data.Entities;

namespace NievoEasyFin.Application.Data.Views;

/// <summary>
/// View for Payment Transaction (alias for TransactionView)
/// </summary>
public class PaymentTransactionView : TransactionView
{
    /// <summary>
    /// Default constructor
    /// </summary>
    public PaymentTransactionView() { }

    /// <summary>
    /// Constructor from entity
    /// </summary>
    /// <param name="entity">TransactionEntity</param>
    public PaymentTransactionView(TransactionEntity entity) : base(entity) { }
}
