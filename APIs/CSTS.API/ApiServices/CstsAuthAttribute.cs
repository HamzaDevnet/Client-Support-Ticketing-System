using CSTS.DAL.Enum;
using CSTS.DAL.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;

namespace CSTS.API.ApiServices
{

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class CstsAuthAttribute : Attribute, IAuthorizationFilter
    {
        private readonly UserType[] _requiredUserType;

        public CstsAuthAttribute(params UserType[] requiredUserType)
        {
            _requiredUserType = requiredUserType;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var userClaims = context.HttpContext.User.Claims;
            var userTypeClaim = userClaims.FirstOrDefault(c => c.Type == "UserType");

            if (userTypeClaim == null || !Enum.TryParse(userTypeClaim.Value, out UserType userType))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            if (!_requiredUserType.Contains(userType))
            {
                context.Result = new UnauthorizedResult();
                return;
            }
        }
    }

}
