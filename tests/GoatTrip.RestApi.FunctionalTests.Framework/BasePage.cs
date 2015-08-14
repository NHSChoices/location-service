

namespace GoatTrip.RestApi.FunctionalTests.Framework
{
    using System;

    public static class BasePage
    {
        public static string Protocol = "http";
        public static string Domain = "localhost";
        public static string Port = "80";
        public static string Route = "";

        public static string BaseRoute
        {
            get { return String.Format("{0}://{1}:{2}/{3}/", Protocol, Domain, Port, Route).TrimEnd("//".ToCharArray()); }
        }

        public static void GoToUrl(string route)
        {
            if (!route.StartsWith("/"))
                route = "/" + route;

            Driver.Instance.Navigate().GoToUrl(BaseRoute + route);
        }
    }
}
