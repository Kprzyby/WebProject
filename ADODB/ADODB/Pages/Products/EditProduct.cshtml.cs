using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ADODB.Models;
using ADODB.DAL;
using System.Data.SqlClient;
using System.Data;
using System.Text.Json;

namespace ADODB.Pages
{
    public class EditModel : PageModel
    {
        public EditModel(IConfiguration configuration, ILogs logs)
        {
            _configuration = configuration;
            this.logs = logs;
            product = new Product();
            CategoryList = new List<Category>();

            string myCompanyDBcs = _configuration.GetConnectionString("MyCompanyDB");

            using (SqlConnection connection = new SqlConnection(myCompanyDBcs))
            {
                connection.Open();
                SqlCommand cmd2 = new SqlCommand("ShowCategory", connection);
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

        [BindProperty]
        public Product product { get; set; }
        public List<Category> CategoryList;
        private readonly IConfiguration _configuration;
        private readonly ILogs logs;

        public void OnGet(int id)
        {
            product.id = id;
        }
        public IActionResult OnPost()
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }
            string myCompanyDBcs = _configuration.GetConnectionString("MyCompanyDB");

            using (SqlConnection connection = new SqlConnection(myCompanyDBcs))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand("EditProduct", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("Id", product.id));
                cmd.Parameters.Add(new SqlParameter("CategoryId", product.CategoryId));
                cmd.Parameters.Add(new SqlParameter("name", product.name));
                cmd.Parameters.Add(new SqlParameter("price", product.price));
                SqlDataReader rd2 = cmd.ExecuteReader();

                rd2.Close();
            }

            logs.CreateLog("EditProduct", JsonSerializer.Serialize(product));
            TempData["Notification"] = "Product updated successfully";

            return RedirectToPage("../Index");
        }
    }
}
