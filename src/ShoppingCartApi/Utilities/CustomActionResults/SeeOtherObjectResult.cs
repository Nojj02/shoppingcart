using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace ShoppingCartApi.Utilities.CustomActionResults
{
    public class SeeOtherObjectResult : ObjectResult
    {
        private readonly string _location;

        public SeeOtherObjectResult(string location, object value) 
            : base(value)
        {
            _location = location;
            StatusCode = (int) HttpStatusCode.SeeOther;
            Value = value;
        }

        public override void OnFormatting(ActionContext context)
        {
            context.HttpContext.Response.Headers.Add(
                Convert.ToString(HttpResponseHeader.Location), 
                new StringValues(_location));
            base.OnFormatting(context);
        }
    }
}