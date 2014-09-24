using dsc.CalendarWidget.Models;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Projections.Models;
using Orchard.Projections.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace dsc.CalendarWidget.Services
{
    public class CalendarService : ICalendarService
    {
        private readonly IProjectionManager _projectionManager;
        private readonly IOrchardServices _orchardServices;

        public CalendarService(IProjectionManager projectionManager, IOrchardServices orchardServices)
        {
            _projectionManager = projectionManager;
            _orchardServices = orchardServices;
        }

        public List<QueryPart> GetCalendarQueries()
        {
            IEnumerable<QueryPart> queryParts = _orchardServices.ContentManager.Query<QueryPart, QueryPartRecord>("Query").List();

            List<QueryPart> calendarQueries = new List<QueryPart>();

            foreach (QueryPart part in queryParts)
            {
                ContentItem contentItem = _projectionManager.GetContentItems(part.Id).FirstOrDefault();

                int countTitleParts = contentItem.TypeDefinition.Parts.Where(r => r.PartDefinition.Name == "TitlePart").Count();
                int countTimeSpanParts = contentItem.TypeDefinition.Parts.Where(r => r.PartDefinition.Name == "TimeSpanPart").Count();

                if ((countTitleParts > 0) && (countTimeSpanParts > 0))
                {
                    calendarQueries.Add(part);
                }
            }

            return calendarQueries;
        }

        public List<CalendarEvent> GetCalendarEvents(CalendarWidgetPart part)
        {
            IEnumerable<ContentItem> contentItems = _projectionManager.GetContentItems(part.QueryId);

            List<CalendarEvent> calendarEvents = new List<CalendarEvent>();

            foreach (ContentItem item in contentItems)
            {
                dynamic record = _orchardServices.ContentManager.Get(item.Record.Id);

                CalendarEvent calendarEvent = new CalendarEvent
                {
                    Title = record.TitlePart.Title,
                    Start = record.TimeSpanPart.StartDateTime.DateTime,
                    End = record.TimeSpanPart.EndDateTime.DateTime,
                    Url = String.Format("Contents/Item/Display/{0}", record.Id),
                    AllDay = record.TimeSpanPart.AllDay.Value
                };

                calendarEvents.Add(calendarEvent);
            }

            return calendarEvents;
        }
    }
}

