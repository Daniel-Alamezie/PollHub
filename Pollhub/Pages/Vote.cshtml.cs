using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Pollhub.Data;

namespace Pollhub.Pages
{
    public class VoteModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public VoteModel(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        [BindProperty]
        public string Key { get; set; }

        public async Task<IActionResult> OnPostAsync(string key)
        {
            // Get the user's IP address
            var ipAddress = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();


            var poll = await _context.Polls.SingleOrDefaultAsync(p => p.Key == Key);

            if (poll == null)
            {
                ModelState.AddModelError("Key", "This key is invalid. Please try a valid key.");
                return Page();
            }

            if (poll.Status == "Closed")
            {
                // Redirect to NoPollPage if poll is closed
                return RedirectToPage("/NoPollPage");
            }


            //Check whether the user has already voted
            if (poll.VoteIPs != null && poll.VoteIPs.Any(v => v.Ip == ipAddress))
            {
                
                // if user already voted, send them to the poll result page
                return RedirectToPage("/PollResults", new { id = poll.Id, poll = poll });
            }

            return RedirectToPage("/Poll", new { id = poll.Id, poll = poll });
        }

        public void OnGet()
        {
           
        }
    }
}
