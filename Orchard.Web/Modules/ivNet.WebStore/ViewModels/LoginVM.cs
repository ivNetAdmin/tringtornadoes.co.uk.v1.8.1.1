using System.ComponentModel.DataAnnotations;

namespace ivNet.Webstore.ViewModels
{
    public class LoginVM
    {
        [Required, Display(Name = "Email address"), DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required, Display(Name = "Password"), DataType(DataType.Password)]
        public string Password { get; set; }

        [UIHint("Checkbox"), Display(Name = "Keep me logged in")]
        public bool CreatePersistentCookie { get; set; }

    }
}