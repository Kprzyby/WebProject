using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ADODB.Models;
using ADODB.DAL;
using System.Data.SqlClient;
using System.Data;
using System.Text.Json;

namespace ADODB.Pages
{
    public class CreateModel : PageModel
    {
        public CreateModel(IConfiguration configuration, ILogs logs)
        {
            product = new Product();
            CategoryList = new List<Category>();
            _configuration = configuration;
            this.logs = logs;

            string ConnectionString = _configuration.GetConnectionString("MyCompanyDB");

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("ShowCategory", con);
                SqlDataReader rd = cmd.ExecuteReader();

                if (rd.HasRows)
                {
                    while (rd.Read())
                    {
                        CategoryList.Add(new Category((int)rd[0], (string)rd[1], (string)rd[2]));
                    }
                }
                rd.Close();
                con.Close();
            }
        }

        [BindProperty]
        public Product product { get; set; }
        public List<Category> CategoryList;
        private readonly IConfiguration _configuration;
        private readonly ILogs logs;

        public void OnGet()
        {
        }
        public IActionResult OnPost()
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }

            string myCompanyDBcs = _configuration.GetConnectionString("MyCompanyDB");

            using (SqlConnection connection=new SqlConnection(myCompanyDBcs))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand("AddProduct", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("CategoryId", product.CategoryId));
                cmd.Parameters.Add(new SqlParameter("name", product.name));
                cmd.Parameters.Add(new SqlParameter("price", product.price));
                cmd.ExecuteNonQuery();

                //the product doesn't have an id yet so in order to create a proper
                //log I have created a query to get the id of the added product
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.Parameters.Add(new SqlParameter("name", product.name));
                command.Parameters.Add(new SqlParameter("price", product.price));
                command.Parameters.Add(new SqlParameter("categoryId", product.CategoryId));
                command.CommandText = "Select Id from Product " +
                    "Where name=@name and price=@price and CategoryId=@categoryId";

                SqlDataReader rd = command.ExecuteReader();
                if (rd.HasRows)
                {
                    rd.Read();
                    product.id = (int)rd[0];
                }
            }

            logs.CreateLog("CreateProduct", JsonSerializer.Serialize(product));
            TempData["Notification"] = "Product created successfully";

            return RedirectToPage("../Index");
        }
    }
}
