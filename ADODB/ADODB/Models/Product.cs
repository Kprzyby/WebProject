using System.ComponentModel.DataAnnotations;

namespace ADODB.Models
{
    public class Product
    {
        public Product(int id, int CategoryId, string name, decimal price)
        {
            this.id = id;
            this.CategoryId = CategoryId;
            this.name = name;
            this.price = price;
        }
        public Product()
        {

        }
        public int id { get; set; }
        public int CategoryId { get; set; }

        [Display(Name ="Name")]
        [Required(ErrorMessage = "This field is required")]
        public string name { get; set; }

        [Display(Name ="Price")]
        [Required(ErrorMessage ="This field is required")]
        [RegularExpression(@"^[+]?\d+([.]\d+)?$", ErrorMessage = "The price must be a positive number")]
        public decimal price { get; set; }
    }
}
