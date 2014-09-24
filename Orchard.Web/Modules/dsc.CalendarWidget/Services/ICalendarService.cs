using dsc.CalendarWidget.Models;
using Orchard;
using Orchard.Projections.Models;
using System.Collections.Generic;

namespace dsc.CalendarWidget.Services
{
    public interface ICalendarService : IDependency
    {
        List<CalendarEvent> GetCalendarEvents(CalendarWidgetPart part);
        List<QueryPart> GetCalendarQueries();
    }
}