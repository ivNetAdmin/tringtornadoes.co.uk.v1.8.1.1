using System;
using Orchard;

namespace ivNet.Webstore.Services {
    public interface IDateTimeService : IDependency {
        DateTime Now { get; }
    }
}