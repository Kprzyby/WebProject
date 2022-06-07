using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ADODB.DAL;

namespace ADODB.Pages.Login
{
    public class LogOutModel : PageModel
    {
        public LogOutModel(ILogs logs)
        {
            this.logs = logs;
        }

        private readonly ILogs logs;

        public async Task<IActionResult> OnGet()
        {
            await HttpContext.SignOutAsync("CookieAuthentication");

            logs.CreateLog("LogOut", null);
            TempData["Notification"] = "You have logged out successfully";

            //deletes the cookie regarding the users agrrement for toastr notifications and the
            //cookie carrying the information whether the user had admin privileges
            if (Request.Cookies["ToastrAgreement"] != null) Response.Cookies.Delete("ToastrAgreement");
            if (Request.Cookies["UserIsAdmin"] != null) Response.Cookies.Delete("UserIsAdmin");

            return this.RedirectToPage("/index");
        }
    }
}
