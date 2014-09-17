using System.Collections.Generic;
using Orchard;
using Orchard.ContentManagement;
using ivNet.Webstore.Models;

namespace ivNet.Webstore.Services {
    public interface ICustomerService : IDependency
    {
        CustomerPart CreateCustomer(string email, string password);
        AddressPart GetAddress(int customerId, string addressType);
        AddressPart CreateAddress(int customerId, string addressType);
        IContentQuery<CustomerPart> GetCustomers();
        CustomerPart GetCustomer(int id);
        IEnumerable<AddressPart> GetAddresses(int customerId);
        AddressPart GetAddress(int id);
    }
}