using System.Security.Cryptography.X509Certificates;
using System.Web.Http.Controllers;

namespace GoatTrip.RestApi.Authentication
{
    public static class CertificateAuthentication
    {
        internal static bool Authorized(HttpActionContext httpContext)
        {
            X509Store certStore = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            certStore.Open(OpenFlags.ReadOnly);
            X509Certificate2Collection certCollection = certStore.Certificates.Find(
                                       X509FindType.FindByThumbprint,
                                       "01879C151E08D3E3901C20A794897EF508F78149",
                                       false);


            // Get the first cert with the thumbprint
            if (certCollection.Count > 0)
            {
                certStore.Close();
                return true;
                //X509Certificate2 cert = certCollection[0];
                //// Use certificate
                //Console.WriteLine(cert.FriendlyName);
            }

            certStore.Close();
            return false;
        }
    }
}