using NievoEasyFin.Application.Data.Views;

namespace NievoEasyFin.Application.Interfaces.Response
{
    public class GetUserCategoryResponse : ResponsePaginationBase<UserCategoryView>
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="page">int</param>
        /// <param name="pageSize">int</param>
        /// <param name="records">int</param>
        /// <param name="view">List UserCategoryView</param>
        public GetUserCategoryResponse(int page, int pageSize, int records, List<UserCategoryView> view) : base(page, pageSize, records, view) { }
    }
}
