using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Http.Hosting;
using System.Web.Http.Routing;
using Moq;

namespace GoatTrip.RestApi.IntegrationTests {
    public static class WebApi {
        public static RouteInfo RouteRequest(HttpConfiguration config, HttpRequestMessage request) {
            // create context
            var controllerContext = new HttpControllerContext(config, Mock.Of<IHttpRouteData>(), request);
            config.EnsureInitialized();

            // get route data
            var routeData = config.Routes.GetRouteData(request);
            RemoveOptionalRoutingParameters(routeData.Values);

            request.Properties[HttpPropertyKeys.HttpRouteDataKey] = routeData;
            controllerContext.RouteData = routeData;

            // get controller type
            var controllerDescriptor = new DefaultHttpControllerSelector(config).SelectController(request);
            controllerContext.ControllerDescriptor = controllerDescriptor;

            // get action name
            var actionMapping = new ApiControllerActionSelector().SelectAction(controllerContext);

            return new RouteInfo {
                Controller = controllerDescriptor.ControllerType,
                Action = actionMapping.ActionName,
                RouteData = controllerContext.RouteData
            };
        }

        private static void RemoveOptionalRoutingParameters(IDictionary<string, object> routeValues) {
            var optionalParams = routeValues
                .Where(x => x.Value == RouteParameter.Optional)
                .Select(x => x.Key)
                .ToList();

            foreach (var key in optionalParams) {
                routeValues.Remove(key);
            }
        }
    }
}