using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Wiki.Data.Domain.Pages.Commands;
using Wiki.Data.Domain.Pages.Queries;
using Wiki.Entities.Common;
using Wiki.Entities.Domain;
using Wiki.Entities.Events;
using Wiki.Entities.Models;
using Wiki.Services.Events;

namespace Wiki.Services.Pages
{
    public class PageService : IPageService
    {
        private readonly IMediator _mediator;

        private readonly IEventService _eventService;

        public PageService(IMediator mediator, IEventService eventService)
        {
            _mediator = mediator;
            _eventService = eventService;
        }

        public async Task<ServiceResult<Page>> GetByIdAsync(int id, int? version)
        {
            var query = new GetById.Query {Id = id};
            var page = await _mediator.SendAsync(query);
            if(page == null) return new ServiceResult<Page>($"Page Id {id} is invalid");
            var latestPage = await GetPageSpecificVersionAsync(page, version);
            return new ServiceResult<Page>(latestPage);
        } 

        public async Task<ServiceResult> CreateAsync(Page page)
        {
            if(page == null) return new ServiceResult("Page was not provided");
            var command = new Insert.Command {Page = page};
            await _mediator.SendAsync(command);
            return new ServiceResult();
        }

        public async Task<ServiceResult> UpdateAsync(Page page)
        {
            if (page == null) return new ServiceResult("Page was not provided");
            if (page.Id == default(int)) return new ServiceResult("Page Id was not provided");
            var existedPage = await GetByIdAsync(page.Id);
            if (existedPage == null) return new ServiceResult($"Page Id {page.Id} is invalid");
            await _eventService.CreateEntityEventAsync(page);
            return new ServiceResult();
        }

        public async Task<ServiceResult<IEnumerable<Page>>> GetAllAsync()
        {
            var pages = (await _mediator.SendAsync(new GetAll.Query()))?.ToList() ?? new List<Page>();
            if(!pages.Any()) return new ServiceResult<IEnumerable<Page>>();

            var updatedPages = new List<Page>();
            foreach (var page in pages)
            {
                var latestPage = await GetPageSpecificVersionAsync(page);
                updatedPages.Add(latestPage);
            }
            return new ServiceResult<IEnumerable<Page>>(updatedPages);
        }

        public async Task<ServiceResult> SetPageInSpecificVersionAsync(int id, int version)
        {
            if(id == default (int)) return new ServiceResult("Pae Id was not provided");
            var page = await GetByIdAsync(id);
            if(page == null) return new ServiceResult($"Page Id {id} is invalid");
            page = version != default(int) ? await GetPageSpecificVersionAsync(page, version) : page;
            return await UpdateAsync(page);
        }

        public async Task<ServiceResult<IEnumerable<PageVersionModel>>> GetAllPageVersionsAsync(int id)
        {
            if(id == default (int)) return new ServiceResult<IEnumerable<PageVersionModel>>("Page id was not provided");
            var page = await GetByIdAsync(id);
            if (page == null) return new ServiceResult<IEnumerable<PageVersionModel>>($"Page Id {id} is invalid");

            //Version list, including ero version (initial state)
            var versions = new List<PageVersionModel> {new PageVersionModel(page)};

            //Add other versions:
            var events = (await _eventService.GetEventsByAggregateIdAsync(id))?.ToList() ?? new List<Event>();
            versions.AddRange(
                from currentEvent in events
                let versionedPage = _eventService.GetPageFromEvent(currentEvent)
                where versionedPage != null
                select new PageVersionModel(
                    currentEvent.Version, 
                    currentEvent.Timestamp, 
                    versionedPage.Text, 
                    versionedPage.Title));

            return new ServiceResult<IEnumerable<PageVersionModel>>(versions);
        }

        private async Task<Page> GetByIdAsync(int id)
        {
            var query = new GetById.Query { Id = id };
            return await _mediator.SendAsync(query);
        }

        private async Task<Page> GetPageSpecificVersionAsync(Page page, int? version = null)
        {          
            var events = await _eventService.GetEventsByAggregateIdAsync(page.Id);
            var pageState = new PageState(page, events, version);
            return pageState.Entity;
        }
    }
}
