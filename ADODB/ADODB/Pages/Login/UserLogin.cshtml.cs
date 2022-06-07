using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ADODB.Models;
using ADODB.DAL;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace ADODB.Pages.Login
{
    public class UserLoginModel : PageModel
    {
        public UserLoginModel(IConfiguration configuration, ILogs logs)
        {
            _configuration = configuration;
            this.logs = logs;
        }

        private readonly IConfiguration _configuration;
        private readonly ILogs logs;

        [BindProperty]
        public SiteUser user { get; set; }

        //informs whether the user agreed for toastr notofications or not, default - true
        [BindProperty]
        public bool ToastrAgreement { get; set; }

        //each user is assigned a role - "admin", "executive" or "employee"
        private string role;

        public void OnGet()
        {
        }

        //the method returns an encrypted password
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
        private bool ValidateUser(SiteUser user)
        {
            //this query selects all the users from the database and if the username and password matches
            //the one submited by the user it returns true
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("MyCompanyDB")))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("Select * from Identification;", con);

                using (SqlDataReader rd = cmd.ExecuteReader())
                {
                    if(rd.HasRows)
                    {
                        while(rd.Read())
                        {
                            if ((user.userName == rd[1].ToString()) && (GetHash(user.password) == rd[2].ToString()))
                            {
                                role = (string)rd[3];
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            if (ValidateUser(user))
            {
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, user.userName),
                    new Claim(ClaimTypes.Role, role)
                };

                var claimsIdentity = new ClaimsIdentity(claims, "CookieAuthentication");
                await HttpContext.SignInAsync("CookieAuthentication", new
                ClaimsPrincipal(claimsIdentity));

                logs.CreateLog("LogIn", user.userName);
                TempData["Notification"] = "Hello "+user.userName+"!";

                var options = new CookieOptions
                {
                    Expires = DateTime.Now.AddMinutes(20)
                };

                if (Request.Cookies["ToastrAgreement"] != null) Response.Cookies.Delete("ToastrAgreement");

                if (ToastrAgreement == true)
                {
                    Response.Cookies.Append("ToastrAgreement", "true", options);
                }

                if (role == "admin") Response.Cookies.Append("UserIsAdmin", "true", options);
            }

            return Page();
        }
    }
}
