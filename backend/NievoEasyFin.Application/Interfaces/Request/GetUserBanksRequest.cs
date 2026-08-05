using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NievoEasyFin.Application.Interfaces.Request
{
    public class GetUserBanksRequest
    {
        /// <summary>
        /// User email
        /// </summary>
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
