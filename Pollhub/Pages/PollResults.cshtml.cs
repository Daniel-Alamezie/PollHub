using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Pollhub.Data;

namespace Pollhub.Pages
{
    public class PollResultsModel : PageModel
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppDbContext _context;
        public PollResultsModel(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public Poll Poll { get; set; }
        public TimeSpan TimeLeft { get; set; }
        public IActionResult OnGet(int id)
        {
            Poll = _context.Polls.Find(id);

            if (Poll == null)
            {
                return NotFound();
            }
            TimeLeft = Poll.ExpirationTime - DateTime.UtcNow;

            return Page();
        }
      

    }
}
