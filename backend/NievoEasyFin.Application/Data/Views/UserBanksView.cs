using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NievoEasyFin.Application.Data.Views
{
    /// <summary>
    /// Class for view user banks
    /// </summary>
    public class UserBanksView
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public UserBanksView() { }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Bank type
        /// </summary>
        public int BankType { get; set; }

        /// <summary>
        /// Nick Name
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// Bank type name
        /// </summary>
        public string BankTypeName { get; set; }
    }
}
