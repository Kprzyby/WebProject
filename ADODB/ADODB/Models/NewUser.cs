using System.ComponentModel.DataAnnotations;

namespace ADODB.Models
{
    public class NewUser
    {
        public int id { get; set; }

        [Display(Name = "Username")]
        [Required(ErrorMessage = "This field is required")]
        public string UserName { get; set; }

        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "This field is required")]
        public string Password1 { get; set; }

        [Display(Name = "Repeat password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "This field is required")]
        [Compare("Password1", ErrorMessage = "Both passwords have to be the same")]
        public string Password2 { get; set; }

        [Display(Name = "Role")]
        [Required(ErrorMessage = "This field is required")]
        public string role { get; set; }
    }
}
