using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using ADODB.Models;
using ADODB.DAL;
using Microsoft.AspNetCore.Identity;
using System.Net;
using System.Text.Json;
using System.ComponentModel.DataAnnotations;

namespace ADODB.Pages
{
    public class IndexModel : PageModel
    {
        public IndexModel(IConfiguration configuration, ContextClass context, ILogger<IndexModel> logger)
        {
            _logger = logger;
            this.context = context;
            _configuration = configuration;
            ProductList = new List<Product>();
            CategoryList = new List<Category>();
        }

        private readonly ILogger<IndexModel> _logger;
        private readonly ContextClass context;
        private readonly IConfiguration _configuration;
        public List<Product> ProductList;
        public List<Category> CategoryList;

        [BindProperty]
        [Required(ErrorMessage = "This field is required")]
        [RegularExpression(@"^[+]?\d+([.]\d+)?$", ErrorMessage = "The price must be a positive number")]
        public decimal lowerPrice { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "This field is required")]
        [RegularExpression(@"^[+]?\d+([.]\d+)?$", ErrorMessage = "The price must be a positive number")]
        public decimal higherPrice { get; set; }

        public void OnGet()
        {
            string myCompanyDBcs = _configuration.GetConnectionString("MyCompanyDB");

            using (SqlConnection con = new SqlConnection(myCompanyDBcs))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("ShowProduct", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ProductList.Add(new Product((int)reader[0],(int)reader[1], (string)reader[2], (decimal)reader[3]));
                    }
                }
                reader.Close();

                SqlCommand cmd2 = new SqlCommand("ShowCategory", con);
                SqlDataReader rd = cmd2.ExecuteReader();

                if (rd.HasRows)
                {
                    while (rd.Read())
                    {
                        CategoryList.Add(new Category((int)rd[0], (string)rd[1], (string)rd[2]));
                    }
                }
                rd.Close();
            }
        }
        public IActionResult OnPostFilterByPrice()
        {
            List<Product> filteredProducts = context.Product.ToList().FindAll(e => e.price >= lowerPrice && e.price <= higherPrice);
            filteredProducts = filteredProducts.OrderBy(e => e.price).ToList();
            return RedirectToPage("FilteredProducts", new { products = JsonSerializer.Serialize(filteredProducts) });
        }
        public IActionResult OnPostFilterByCategory(int categoryId)
        {
            List<Product> filteredProducts = context.Product.ToList().FindAll(e => e.CategoryId == categoryId);
            return RedirectToPage("FilteredProducts", new { products = JsonSerializer.Serialize(filteredProducts) });
        }
    }
}