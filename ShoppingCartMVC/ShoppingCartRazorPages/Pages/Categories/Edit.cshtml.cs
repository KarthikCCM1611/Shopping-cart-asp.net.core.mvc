using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShoppingCartRazorPages.Data;
using ShoppingCartRazorPages.Model;

namespace ShoppingCartRazorPages.Pages.Categories
{

    [BindProperties]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public Category Category { get; set; }

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public void OnGet(int id)
        {
            if (id != 0 || id != null)
            {
                Category = _context.Categories.Find(id);
            }
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            _context.Categories.Update(Category);
            _context.SaveChanges();
            TempData["success"] = "Category updated successfully";
            return RedirectToPage("Index");
        }
    }
}
