using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ivNet.Webstore.ViewModels {
    public class AddressesVM : IValidatableObject {

        [UIHint("Address"), Display(Name = "Address")]
        public AddressVM InvoiceAddress { get; set; }

        [UIHint("Address"), Display(Name = "Shipping Address (optional)")]
        public AddressVM ShippingAddress { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
            var address = InvoiceAddress;

            if(string.IsNullOrWhiteSpace(address.AddressLine1))
                yield return new ValidationResult("Addressline 1 is a required field", new[]{"InvoiceAddress.AddressLine1"});

            if (string.IsNullOrWhiteSpace(address.Zipcode))
                yield return new ValidationResult("Zipcode is a required field", new[] { "InvoiceAddress.Zipcode" });

            if (string.IsNullOrWhiteSpace(address.City))
                yield return new ValidationResult("City is a required field", new[] { "InvoiceAddress.City" });

            if (string.IsNullOrWhiteSpace(address.Country))
                yield return new ValidationResult("Country is a required field", new[] { "InvoiceAddress.Country" });
        }
    }
}