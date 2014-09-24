using Orchard.UI.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dsc.CalendarWidget
{
    public class ResourceManifest:IResourceManifestProvider
    {
        public void BuildManifests(ResourceManifestBuilder builder)
        {
            var manifest = builder.Add();

            manifest.DefineScript("Moment").SetUrl("ext/fullcalendar/moment.min.js").SetDependencies("jQuery");
            manifest.DefineScript("LangAll").SetUrl("ext/fullcalendar/lang-all.js");
            manifest.DefineScript("FullCalendar").SetUrl("ext/fullcalendar/fullcalendar.min.js", "ext/fullcalendar/fullcalendar.js").SetDependencies("Moment", "jQuery");
            manifest.DefineScript("Event").SetUrl("Event.min.js", "Event.js");
            manifest.DefineScript("CalendarWidget").SetUrl("CalendarWidget.min.js", "CalendarWidget.js").SetDependencies("FullCalendar","Event");

            manifest.DefineStyle("FullCalendar").SetUrl("ext/fullcalendar/FullCalendar.min.css", "ext/fullcalendar/FullCalendar.css");
            manifest.DefineStyle("FullCalendarPrint").SetUrl("ext/fullcalendar/FullCalendar.print.min.css", "ext/fullcalendar/FullCalendar.print.css");
            manifest.DefineStyle("CalendarWidget").SetUrl("CalendarWidget.min.css", "CalendarWidget.css").SetDependencies("FullCalendar");
        }
    }
}