using System.Collections.Generic;
using System.Linq;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Security;
using Orchard.Users.Models;
using ivNet.Webstore.Models;

namespace ivNet.Webstore.Services {
    public class CustomerService : ICustomerService {
        private readonly IOrchardServices _orchardServices;
        private readonly IMembershipService _membershipService;
        private readonly IDateTimeService _dateTimeService;

        public CustomerService(IOrchardServices orchardServices, IMembershipService membershipService, IDateTimeService dateTimeService) {
            _orchardServices = orchardServices;
            _membershipService = membershipService;
            _dateTimeService = dateTimeService;
        }

        public CustomerPart CreateCustomer(string email, string password) {
            var customer = _orchardServices.ContentManager.New("Customer");
            var userPart = customer.As<UserPart>();
            var customerPart = customer.As<CustomerPart>();

            userPart.UserName = email;
            userPart.Email = email;
            userPart.NormalizedUserName = email.ToLowerInvariant();
            userPart.Record.HashAlgorithm = "SHA1";
            userPart.Record.RegistrationStatus = UserStatus.Approved;
            userPart.Record.EmailStatus = UserStatus.Approved;

            customerPart.CreatedAt = _dateTimeService.Now;

            _membershipService.SetPassword(userPart, password);
            _orchardServices.ContentManager.Create(customer);

            return customerPart;
        }

        public AddressPart GetAddress(int customerId, string addressType) {
            return _orchardServices.ContentManager.Query<AddressPart, AddressRecord>().Where(x => x.CustomerId == customerId && x.Type == addressType).List().FirstOrDefault();
        }

        public AddressPart GetAddress(int id) {
            return _orchardServices.ContentManager.Get<AddressPart>(id);
        }

        public AddressPart CreateAddress(int customerId, string addressType) {
            return _orchardServices.ContentManager.Create<AddressPart>("Address", x => {
                x.Type = addressType;
                x.CustomerId = customerId;
            });
        }

        public IContentQuery<CustomerPart> GetCustomers() {
            return _orchardServices.ContentManager.Query<CustomerPart, CustomerRecord>();
        }

        public CustomerPart GetCustomer(int id) {
            return _orchardServices.ContentManager.Get<CustomerPart>(id);
        }

        public IEnumerable<AddressPart> GetAddresses(int customerId) {
            return _orchardServices.ContentManager.Query<AddressPart, AddressRecord>().Where(x => x.CustomerId == customerId).List();
        }

        
    }
}