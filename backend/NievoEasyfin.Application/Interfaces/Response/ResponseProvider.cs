using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NievoEasyfin.Application.Interfaces.Response
{
    public class ResponseProvider
    {
        /// <summary>
        /// Has some error the request
        /// </summary>
        public string? Error { get; set; }

        public void WithError(string error)
        {
            Error = error;
        }
    }
}