using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Data;
using ADODB.DAL;
using ADODB.Models;
using System.Text.Json;

namespace ADODB.Pages
{
    public class DeleteModel : PageModel
    {
        public DeleteModel(IConfiguration configuration, ILogs logs)
        {
            _configuration = configuration;
            this.logs = logs;
        }

        private readonly IConfiguration _configuration;
        private readonly ILogs logs;

        public IActionResult OnGet(int id)
        {
            Product product = new Product();

            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("MyCompanyDB")))
            {
                con.Open();

                //this query gets all the information about the deleted product so that a
                //proper log can be created
                SqlCommand cmd1 = new SqlCommand("Select * from Product where id=@id", con);
                cmd1.Parameters.Add(new SqlParameter("id", id));

                using (SqlDataReader rd = cmd1.ExecuteReader())
                {
                    if(rd.HasRows)
                    {
                        rd.Read();
                        product.id = (int)rd[0];
                        product.CategoryId = (int)rd[1];
                        product.name = (string)rd[2];
                        product.price = (decimal)rd[3];
                    }
                }

                SqlCommand cmd2 = new SqlCommand("DeleteProduct", con);
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.Parameters.Add(new SqlParameter("Id", id));
                cmd2.ExecuteNonQuery();
            }

            logs.CreateLog("DeleteProduct", JsonSerializer.Serialize(product));
            TempData["Notification"] = "Product deleted successfully";

            return RedirectToPage("../Index");
        }
    }
}
