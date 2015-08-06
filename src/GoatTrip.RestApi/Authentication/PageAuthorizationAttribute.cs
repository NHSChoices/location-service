using System;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace GoatTrip.RestApi.Authentication
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class Authorize : AuthorizeAttribute
    {
        protected override bool IsAuthorized(HttpActionContext httpContext) {
            return CertificateAuthentication.Authorized(httpContext);
        }
    }
}