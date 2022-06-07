using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ADODB.DAL;
using ADODB.Models;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace ADODB.Pages
{
    public class AddUserModel : PageModel
    {
        public AddUserModel(IConfiguration configuration, ILogs logs)
        {
            this.configuration = configuration;
            this.logs = logs;
            roles = new List<string>
            {
                "admin",
                "executive",
                "employee"
            };
        }

        [BindProperty]
        public NewUser user { get; set; }
        private readonly IConfiguration configuration;
        private readonly ILogs logs;

        //a list of roles that can be assigned to a user
        public List<string> roles { get; set; }

        public void OnGet()
        {
        }
        //this method returns an encrypted password
        private static string GetHash(string input)
        {
            SHA256 hashAlgorithm = SHA256.Create();
            byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));

            var sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) return Page();

            using (SqlConnection con = new SqlConnection(configuration.GetConnectionString("MyCompanyDB")))
            {
                con.Open();

                SqlCommand command = new SqlCommand("AddUser", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("login", user.UserName));
                command.Parameters.Add(new SqlParameter("password", GetHash(user.Password1)));
                command.Parameters.Add(new SqlParameter("role", user.role));

                command.ExecuteNonQuery();
            }

            TempData["Notification"] = "User created successfully";
            logs.CreateLog("CreateUser", JsonSerializer.Serialize(user));

            return RedirectToPage("Index");
        }
    }
}
