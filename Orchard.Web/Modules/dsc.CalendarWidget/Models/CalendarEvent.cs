using System;

namespace dsc.CalendarWidget.Models
{
    public class CalendarEvent
    {
        public string Title { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public bool? AllDay { get; set; }
        public string Url { get; set; }
    }
}
