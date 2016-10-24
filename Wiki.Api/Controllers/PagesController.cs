using System.Threading.Tasks;
using System.Web.Http;
using Wiki.Entities.Domain;
using Wiki.Services.Pages;

namespace Wiki.Api.Controllers
{
    [RoutePrefix("api/pages")]
    public class PagesController : BaseApiController
    {
        private readonly IPageService _pageService;

        public PagesController(IPageService pageService)
        {
            _pageService = pageService;
        }

        /// <summary>
        /// Get all pages
        /// </summary>
        /// <returns></returns>
        public async Task<IHttpActionResult> Get()
        {
            var serviceResult = await _pageService.GetAllAsync();
            return GetErrorResult(serviceResult) ?? Ok(serviceResult.Result);
        }

        /// <summary>
        /// Get page by Id and Version
        /// </summary>
        /// <param name="id">Page Id</param>
        /// <param name="version">Version Nr</param>
        /// <returns></returns>
        [Route("{id}")]
        public async Task<IHttpActionResult> Get(int id, int? version = null)
        {
            var serviceResult = await _pageService.GetByIdAsync(id, version);
            return GetErrorResult(serviceResult) ?? Ok(serviceResult.Result);
        }

        /// <summary>
        /// Create new Page
        /// </summary>
        /// <param name="page">Page to create</param>
        /// <returns></returns>
        public async Task<IHttpActionResult> Post(Page page)
        {
            var serviceResult = await _pageService.CreateAsync(page);
            return GetErrorResult(serviceResult) ?? Ok();
        }

        /// <summary>
        /// Update Page
        /// </summary>
        /// <param name="page">Page object to update</param>
        /// <returns></returns>
        public async Task<IHttpActionResult> Put(Page page)
        {
            var serviceResult = await _pageService.UpdateAsync(page);
            return GetErrorResult(serviceResult) ?? Ok();
        }

        /// <summary>
        /// Get all page versions
        /// </summary>
        /// <param name="id">Page Id</param>
        /// <returns></returns>
        [Route("{id}/versions")]
        public async Task<IHttpActionResult> GetPageVersions(int id)
        {
            var serviceResult = await _pageService.GetAllPageVersionsAsync(id);
            return GetErrorResult(serviceResult) ?? Ok(serviceResult.Result);
        }

        /// <summary>
        /// Change default page vesion
        /// </summary>
        /// <param name="id">Page Id</param>
        /// <param name="version">Version Nr</param>
        /// <returns></returns>
        [HttpPost, Route("{id}/versions/{version}")]
        public async Task<IHttpActionResult> SetPageVersion(int id, int version)
        {
            var serviceResult = await _pageService.SetPageInSpecificVersionAsync(id, version);
            return GetErrorResult(serviceResult) ?? Ok();
        }
        
    }
}
