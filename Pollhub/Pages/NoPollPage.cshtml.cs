using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Pollhub.Data;

namespace Pollhub.Pages
{
    public class NoPollPageModel : PageModel
    {
        private readonly AppDbContext _context;
        public NoPollPageModel (AppDbContext context)
        {
            _context = context;
        }
        public Poll Poll { get; set; }

        public void OnGet(int id)
        {
            Poll = _context.Polls.FirstOrDefault(p => p.Id == id);
        }
    }
}
