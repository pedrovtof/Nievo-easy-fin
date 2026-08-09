using System.Text.Json.Serialization;
using NievoEasyFin.Application.Data.Entities;
using NievoEasyFin.Application.Data.Views;

namespace NievoEasyFin.Application.Interfaces.Response
{
    public class GetCardTypeResponse : ResponsePaginationBase<BankCardTypeView>
    {
        public GetCardTypeResponse(int page, int pageSize, int records, List<BankCardTypeView> view) : base(page, pageSize, records, view) { }
    }
}
