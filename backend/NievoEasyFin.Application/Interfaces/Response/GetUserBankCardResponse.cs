using NievoEasyFin.Application.Data.Views;

namespace NievoEasyFin.Application.Interfaces.Response;

/// <summary>
/// Class API interface for get bank card
/// </summary>
public class GetUserBankCardResponse : ResponsePaginationBase<UserBankCardView>
{
    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="page">int</param>
    /// <param name="pageSize">int</param>
    /// <param name="records">int</param>
    /// <param name="view">List UserBankCardView</param>
    public GetUserBankCardResponse(int page, int pageSize, int records, List<UserBankCardView> view) : base(page, pageSize, records, view) { }
}
