using System.Collections.Generic;
using System.Threading.Tasks;
using Wiki.Entities.Common;
using Wiki.Entities.Domain;
using Wiki.Entities.Models;

namespace Wiki.Services.Pages
{
    public interface IPageService
    {
        Task<ServiceResult<Page>> GetByIdAsync(int id, int? version);
        Task<ServiceResult> CreateAsync(Page page);
        Task<ServiceResult> UpdateAsync(Page page);
        Task<ServiceResult<IEnumerable<Page>>> GetAllAsync();
        Task<ServiceResult> SetPageInSpecificVersionAsync(int id, int version);
        Task<ServiceResult<IEnumerable<PageVersionModel>>> GetAllPageVersionsAsync(int id);
    }
}
