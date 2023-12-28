using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShoppingCartRazorPages.Data;
using ShoppingCartRazorPages.Model;

namespace ShoppingCartRazorPages.Pages.Categories
{
    [BindProperties]
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public Category Category { get; set; }

        public DeleteModel(ApplicationDbContext context)
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
            Category categoryObj = _context.Categories.Find(Category.Id);
            if (categoryObj == null)
            {
                return NotFound();
            }
            _context.Categories.Remove(categoryObj);
            _context.SaveChanges();
            TempData["success"] = "Category deleted successfully";
            return RedirectToPage("Index");
        }
    }
}
