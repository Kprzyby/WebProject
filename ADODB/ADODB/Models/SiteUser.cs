using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ADODB.Models
{
    public class SiteUser
    {
        [Display(Name = "Username")]
        [Required(ErrorMessage ="This field is required")]
        public string userName { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage ="This field is required")]
        [DataType(DataType.Password)]
        public string password { get; set; }
    }
}