using System.Collections.Generic;
using Newtonsoft.Json;
using Wiki.Entities.Events;

namespace Wiki.Entities.Domain
{
    public class PageState : State<Page>
    {
        public PageState(Page entity, IEnumerable<Event> events, int? version = null) : base(entity, events, version)
        {
        }

        protected override void Mutate(Event currentEvent)
        {
            if(string.IsNullOrWhiteSpace(currentEvent?.Data)) return;
            var page = JsonConvert.DeserializeObject<Page>(currentEvent.Data);
            if(page == null) return;
            Entity.Title = page.Title;
            Entity.Text = page.Text;
        }
    }
}
