using System;

namespace ivNet.Webstore.Services {
    public class DateTimeService : IDateTimeService {
        public DateTime Now {
            get { return DateTime.UtcNow; }
        }
    }
}