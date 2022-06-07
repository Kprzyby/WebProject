using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ADODB.Models;
using System.Data;
using System.Data.SqlClient;
using ADODB.DAL;
using System.Text.Json;

namespace ADODB.Pages.Categories
{
    public class DeleteModel : PageModel
    {
        public DeleteModel(IConfiguration configuration, ContextClass context, ILogs logs)
        {
            _configuration = configuration;
            this.logs = logs;
            this.context = context;
        }

        private readonly IConfiguration _configuration;
        private readonly ILogs logs;
        private readonly ContextClass context;

        public IActionResult OnGet(int id)
        {
            Category category=context.Category.Find(id);
            context.Category.Remove(category);
            context.SaveChanges();

            logs.CreateLog("DeleteCategory", JsonSerializer.Serialize(category));
            TempData["Notification"] = "Category deleted successfully";

            return RedirectToPage("Index");
        }
    }
}
