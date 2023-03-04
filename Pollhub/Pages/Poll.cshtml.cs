using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Pollhub.Data;
using System.Reflection.Metadata.Ecma335;

namespace Pollhub.Pages
{
    //Fetch the id of of the poll using the key that user has entered
    public class PollModel : PageModel
    {
        //new instace of the db
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public PollModel(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public Poll Poll { get; set; }

        public IActionResult OnGet(int id)
        {
            Poll = _context.Polls.Find(id);

            if (Poll == null)
            {
                return NotFound();
            }

            return Page();
        }

        //Finds the ID associated with the Key and add a vote to the option that has a vote
        public IActionResult OnPost(int id, string option)
        {
            var poll = _context.Polls.Find(id);

            if (poll == null)
            {
                return NotFound();
            }


            // Get the user's IP address
            var ipAddress = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();

            //Check whether the user has already voted
            if (poll.VoteIPs != null && poll.VoteIPs.Any(v => v.Ip == ipAddress))
            {
                // User has already voted, so return an error
                //return BadRequest("You have already voted in this poll.");
                return RedirectToPage("/PollResults", new { id });
            }

            // Find the options in the db using the Id related to the key retrived by user
            var selectedOption = poll.Options?.FirstOrDefault(o => o.Text == option);

            if (selectedOption == null)
            {
                return BadRequest("Invalid option selected.");
            }


            // Add the user's IP address to the list of voter IPs
            if (poll.VoteIPs == null)
            {
                poll.VoteIPs = new List<VoteIPs>();
            }

            poll.VoteIPs.Add(new VoteIPs { Ip = ipAddress });

            // Update the option vote count
            selectedOption.Votes++;

            // Save the new poll detail in the db
            _context.Polls.Update(poll);
            _context.SaveChanges();

            return RedirectToPage("/PollResults", new { id });
        }


    }
}
