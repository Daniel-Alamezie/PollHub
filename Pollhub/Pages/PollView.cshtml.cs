using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Pollhub.Data;

namespace Pollhub.Pages
{
    public class PollViewModel : PageModel
    {
        private readonly AppDbContext _context;

        public PollViewModel(AppDbContext context)
        {
            _context = context;
        }

        public Poll Poll { get; set; }

        public IActionResult OnGet(int id)
        {
            Poll = _context.Polls.FirstOrDefault(p => p.Id == id);
            if (Poll == null)
            {
                return NotFound();
            }

            return Page();
        }

       
    }
}
