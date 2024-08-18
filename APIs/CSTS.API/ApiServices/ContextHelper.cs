using CSTS.DAL.Enum;
using CSTS.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Security.Claims;

namespace CSTS.API.ApiServices
{
    public static class ContextHelper
    {

        public static UserType GetCurrentUserType(this ControllerBase controllerBase)
        {
            var i = controllerBase.User.Identity as ClaimsIdentity;
            string x = i.FindFirst("UserType")?.Value ?? "";

            UserType result;
            Enum.TryParse(x, out result);
            return result;
        }

        public static Guid GetCurrentUserId(this ControllerBase controllerBase)
        {
            return Guid.Parse(controllerBase.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }

    }
}
