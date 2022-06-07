using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ADODB.Models;
using ADODB.DAL;
using System.Data.SqlClient;
using System.Data;
using System.Text.Json;

namespace ADODB.Pages.Categories
{
    public class EditModel : PageModel
    {
        public EditModel(IConfiguration configuration, ContextClass context, ILogs logs)
        {
            _configuration = configuration;
            this.context = context;
            this.logs = logs;
            Category = new Category();
        }

        private readonly IConfiguration _configuration;
        private readonly ContextClass context;
        private readonly ILogs logs;

        [BindProperty]
        public Category Category { get; set; }

        public void OnGet(int id)
        {
            Category.Id = id;
            Category = context.Category.Find(id);
        }
        public async Task<IActionResult> OnPost()
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }

            context.Category.Update(Category);
            await context.SaveChangesAsync();

            logs.CreateLog("EditCategory", JsonSerializer.Serialize(Category));
            TempData["Notification"] = "Category updated successfully";

            return RedirectToPage("Index");
        }
    }
}
