using NievoEasyFin.Application.Data.Views;

namespace NievoEasyFin.Application.Interfaces.Response
{
    public class GetUserGoalResponse : ResponsePaginationBase<UserGoalView>
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="page">int</param>
        /// <param name="pageSize">int</param>
        /// <param name="records">int</param>
        /// <param name="view">List UserGoalView</param>
        public GetUserGoalResponse(int page, int pageSize, int records, List<UserGoalView> view) : base(page, pageSize, records, view) { }
    }
}
