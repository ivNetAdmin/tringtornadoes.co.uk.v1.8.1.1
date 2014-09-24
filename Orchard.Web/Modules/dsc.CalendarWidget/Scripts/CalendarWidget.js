/// <reference path="ext/fullcalendar/ifullcalendar.ts" />
/// <reference path="typings/jquery/jquery.d.ts" />
/// <reference path="event.ts" />
var dsc;
(function (dsc) {
    var CalendarWidget = (function () {
        function CalendarWidget(Id, Events) {
            var fullCalEvents = [];

            var iterator = function (event) {
                var newEvent;

                newEvent = new dsc.Event(event.title, event.start, event.end, event.url, event.allDay);

                fullCalEvents.push(newEvent);
            };

            Events.forEach(iterator);

            $('#' + Id).fullCalendar({
                lang: navigator.userLanguage,
                theme: false,
                header: {
                    left: 'title',
                    center: '',
                    right: 'prev,next today'
                },
                editable: false,
                timezone: 'local',
                events: fullCalEvents
            });
        }
        return CalendarWidget;
    })();
    dsc.CalendarWidget = CalendarWidget;
})(dsc || (dsc = {}));
