using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ADODB.Models;
using ADODB.DAL;
using System.Data.SqlClient;
using System.Data;
using System.Text.Json;

namespace ADODB.Pages.Categories
{
    public class CreateModel : PageModel
    {
        public CreateModel(IConfiguration configuration, ContextClass context, ILogs logs)
        {
            Category = new Category();
            _configuration = configuration;
            this.context = context;
            this.logs = logs;
        }

        [BindProperty]
        public Category Category { get; set; }
        private readonly IConfiguration _configuration;
        private readonly ContextClass context;
        private readonly ILogs logs;

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }

            await context.Category.AddAsync(Category);
            await context.SaveChangesAsync();

            //the category doesn't have an id yet so in order to create a proper
            //log I have created a query to get the id of the added category
            string ConnectionString = _configuration.GetConnectionString("MyCompanyDB");

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = con;
                command.Parameters.Add(new SqlParameter("shortName", Category.shortName));
                command.Parameters.Add(new SqlParameter("longName", Category.longName));
                command.CommandText = "Select Id from Category " +
                    "Where shortName=@shortName and longName=@longName";

                using (SqlDataReader rd = command.ExecuteReader())
                {
                    if (rd.HasRows)
                    {
                        rd.Read();
                        Category.Id = (int)rd[0];
                    }
                }
            }

            logs.CreateLog("CreateCategory", JsonSerializer.Serialize(Category));
            TempData["Notification"]="Category created successfully";

            return RedirectToPage("Index");
        }
    }
}
