using System.Web.Http;
using Wiki.Entities.Common;

namespace Wiki.Api.Controllers
{
    public class BaseApiController : ApiController
    {
        protected IHttpActionResult GetErrorResult(ServiceResult result)
        {
            if (result == null) return InternalServerError();
            if (result.Succeeded) return null;

            if (result.Errors != null)
            {
                int count = 0;
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError($"Error_{++count}", error);
                }
            }

            if (ModelState.IsValid) return BadRequest();
            return BadRequest(ModelState);
        }
    }
}
