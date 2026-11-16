using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NievoEasyFin.Application.Interfaces.Request
{
    /// <summary>
    /// Request contract for retrieving authenticated user details via JWT email claim.
    /// </summary>
    public class GetUserDetailsRequest : ClaimRequestBase
    {

    }
}