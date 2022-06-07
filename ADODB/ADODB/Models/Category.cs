using System.ComponentModel.DataAnnotations;

namespace ADODB.Models
{
    public class Category
    {
        public Category()
        {

        }
        public Category(int id, string shortName, string longName)
        {
            Id = id;
            this.shortName = shortName;
            this.longName = longName;
        }
        public int Id { get; set; }

        [Display(Name ="Short name")]
        [Required(ErrorMessage ="This atribute is required")]
        public string shortName { get; set; }

        [Display(Name ="Long name")]
        [Required(ErrorMessage = "This atribute is required")]
        public string longName { get; set; }
    }
}
