using System.Collections.Generic;
using Orchard.Projections.Models;

namespace dsc.CalendarWidget.Models
{
    public class CalendarWidgetPartQueries
    {
        public IEnumerable<QueryPart> Queries { get; set; }
        public int QueryId { get; set; }
    }
}