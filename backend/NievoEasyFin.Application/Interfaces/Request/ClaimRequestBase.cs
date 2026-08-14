namespace NievoEasyFin.Application.Interfaces.Request
{
    public class ClaimRequestBase
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