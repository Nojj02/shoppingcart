using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace ShoppingCartApi.Tests.Helpers
{
    public class AlwaysEmptyUrlHelper : IUrlHelper
    {
        public string Action(UrlActionContext actionContext)
        {
            return String.Empty;
        }

        public string Content(string contentPath)
        {
            return String.Empty;
        }

        public bool IsLocalUrl(string url)
        {
            return true;
        }

        public string RouteUrl(UrlRouteContext routeContext)
        {
            return String.Empty;
        }

        public string Link(string routeName, object values)
        {
            return String.Empty;
        }

        public ActionContext ActionContext { get; }
    }
}