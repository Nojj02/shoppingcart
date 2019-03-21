using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace ShoppingCartApi.Tests.Helpers
{
    public class ActionNameOnlyUrlHelper : IUrlHelper
    {
        public ActionContext ActionContext { get; }

        public string Action(UrlActionContext actionContext)
        {
            return actionContext.Action;
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
    }
}