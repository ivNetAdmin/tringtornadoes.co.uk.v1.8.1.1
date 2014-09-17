using System.ComponentModel.DataAnnotations;

namespace ivNet.Webstore.ViewModels
{
    public class AddressVM
    {
        [StringLength(50), Display(Name = "Name")]
        public string Name { get; set; }

        [StringLength(256), Display(Name = "Addressline 1")]
        public string AddressLine1 { get; set; }

        [StringLength(256), Display(Name = "Addressline 2")]
        public string AddressLine2 { get; set; }

        [StringLength(10), Display(Name = "Zipcode")]
        public string Zipcode { get; set; }

        [StringLength(50), Display(Name = "City")]
        public string City { get; set; }

        [StringLength(50), Display(Name = "Country")]
        public string Country { get; set; }
    }
}