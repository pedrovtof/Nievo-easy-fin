using NievoEasyFin.Application.Data.Views;

namespace NievoEasyFin.Application.Interfaces.Response
{
    public class GetCardFlagesponse : ResponsePaginationBase<BankCardFlagView>
    {
        public GetCardFlagesponse(int page, int pageSize, int records, List<BankCardFlagView> view) : base(page, pageSize, records, view) { }
    }
}
