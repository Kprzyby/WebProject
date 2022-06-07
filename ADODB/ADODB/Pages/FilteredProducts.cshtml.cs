using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ADODB.Models;
using ADODB.DAL;
using System.Text.Json;

namespace ADODB.Pages
{
    public class FilteredProductsModel : PageModel
    {
        public FilteredProductsModel(ContextClass context)
        {
            categories = context.Category.ToList();
        }
        public List<Product> filteredproducts { get; set; }
        public List<Category> categories;
        public void OnGet(string products)
        {
            filteredproducts = JsonSerializer.Deserialize<List<Product>>(products);
        }
    }
}
