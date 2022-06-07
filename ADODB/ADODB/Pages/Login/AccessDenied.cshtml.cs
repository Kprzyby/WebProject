using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ADODB.Pages.Login
{
    public class AccessDeniedModel : PageModel
    {
        //the path to redirect the user to
        public string GoBackPath { get; set; }
        public IActionResult OnGet()
        {
            //the path of the page the user came from
            string sourcePath = HttpContext.Request.Headers["Referer"];

            //the user could come from either the index page for products or the index page for categories
            if (sourcePath.Contains("Categories")) GoBackPath = "/Categories/Index";
            else GoBackPath = "/Index";

            //if the user agreed to get the toastr notifications he will get redirected and get a
            //notification about not having access to this page, he will not view the content of this
            //page
            if (Request.Cookies["ToastrAgreement"]!=null)
            {
                TempData["AccessDenied"] = "You don't have access to this page!";
                return RedirectToPage(GoBackPath);
            }

            return null;
        }
    }
}
