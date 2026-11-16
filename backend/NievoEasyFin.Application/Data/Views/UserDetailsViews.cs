using System.Text.Json.Serialization;

namespace NievoEasyFin.Application.Data.Views
{
    /// <summary>
    /// View model representing user profile details joined with status metadata.
    /// </summary>
    public class UserDetailsViews
    {
        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Phone
        /// </summary>
        public int? Phone { get; set; }

        /// <summary>
        /// UserStatus
        /// </summary>
        public string UserStatus { get; set; }
    }
}
