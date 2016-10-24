using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using Wiki.Entities.Common;

namespace Wiki.Api.Filters
{
    public class WikiErrorHandler : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            string errorMessage = ErrorUtils.GetErrorMessage(context.Exception, "Service error");
                var response = new ServiceResult(errorMessage);
            
            context.Response = context.Request.CreateResponse(HttpStatusCode.BadRequest, response);
            base.OnException(context);
        }
    }
}