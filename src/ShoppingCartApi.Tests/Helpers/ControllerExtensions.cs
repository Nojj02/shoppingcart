using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace ShoppingCartApi.Tests.Helpers
{
    public static class ControllerExtensions
    {
        public static T BootstrapForTests<T>(this T controller,
            IUrlHelper urlHelper = null)
            where T : ControllerBase
        {
            controller.Url = urlHelper ?? new AlwaysEmptyUrlHelper();

            return controller;
        }
    }
}
