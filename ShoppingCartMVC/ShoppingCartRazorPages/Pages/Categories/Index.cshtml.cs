using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShoppingCartRazorPages.Data;
using ShoppingCartRazorPages.Model;

namespace ShoppingCartRazorPages.Pages.Categories
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public List<Category> CategoryList { get; set; }

        public IndexModel(ApplicationDbContext context)
        {
            _context= context;
        }

        public void OnGet()
        {
            CategoryList = _context.Categories.ToList();
        }
    }
}
