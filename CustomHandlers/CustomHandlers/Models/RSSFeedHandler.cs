using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;

namespace CustomHandlers.Models
{
    public class RSSFeedHandler : IRouteHandler
    {
        private IControllerFactory _controllerFactory;

        public RSSFeedHandler()
        {
        }

        public RSSFeedHandler(IControllerFactory controllerFactory)
        {
            _controllerFactory = controllerFactory;
        }

        protected virtual IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            requestContext.HttpContext.SetSessionStateBehavior(
                           GetSessionStateBehavior(requestContext));
            return new MvcHandler(requestContext);
        }

        protected virtual SessionStateBehavior
        GetSessionStateBehavior(RequestContext requestContext)
        {
            string controllerName =
            (string)requestContext.RouteData.Values["controller"];
            IControllerFactory controllerFactory = _controllerFactory ??
                           ControllerBuilder.Current.GetControllerFactory();
            return controllerFactory.GetControllerSessionBehavior
                    (requestContext, controllerName);
        }

        #region IRouteHandler Members

        IHttpHandler IRouteHandler.GetHttpHandler(RequestContext requestContext)
        {
            return GetHttpHandler(requestContext);
        }

        #endregion

        //public IHttpHandler GetHttpHandler(RequestContext requestContext)
        //{
        //    var routeValues = requestContext.RouteData.Values;
        //    if (routeValues.ContainsKey("UniqueIdentifier"))
        //    {
        //        requestContext.HttpContext.Response.Clear();

        //        requestContext.HttpContext.Response.Redirect("RSSFeed/Index");

        //        ////Response type will be the same as the one requested
        //        //requestContext.HttpContext.Response.ContentType = GetContentType(requestContext.HttpContext.Request.Url.ToString());

        //        //We buffer the data to send back until it's done
        //        requestContext.HttpContext.Response.BufferOutput = true;

        //        requestContext.HttpContext.Response.End();
        //        var httpHandler = new RSSFeedHttpHandler();
               
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
    }

    public class RSSFeedHttpHandler : IHttpHandler
    {
        public bool IsReusable
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void ProcessRequest(HttpContext httpContext)
        {
            HttpContextBase iHttpContext = new HttpContextWrapper(httpContext);
            ProcessRequest(iHttpContext);
        }

        public void ProcessRequest(HttpContextBase httpContext)
        {
            SecurityUtil.ProcessInApplicationTrust(() => {
                IController controller;
                IControllerFactory factory;
                ProcessRequestInit(httpContext, out controller, out factory);

                try
                {
                    controller.Execute(RequestContext);
                }
                finally
                {
                    factory.ReleaseController(controller);
                }
            });
        }
    }
}