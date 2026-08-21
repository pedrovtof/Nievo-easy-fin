namespace NievoEasyFin.Application.Interfaces.Request
{
    public class PaginationClaimRequestBase : PaginationRequestBase
    {
        private string Email;

        public string GetEmail()
        {
            return Email;
        }

        public void SetEmail(string email)
        {
            Email = email;
        }
    }
}
