using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using Pollhub.Data;
using System.Runtime.CompilerServices;

namespace Pollhub.Pages
{

    public class CreateModel : PageModel
    {

        private readonly AppDbContext _context;

        public CreateModel(AppDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
        }


        // generate a random key to show user after a poll has been created
        private string GenerateKey()
        {
            const string characters = "ABCDEFGHIJKLMNOPQRSTUV0123456789";
            var random = new Random();

            while (true)
            {
                // generate 10 random characters to use as key
                var key = new string(Enumerable.Repeat(characters, 10).Select(s => s[random.Next(s.Length)]).ToArray());

                // check if the key already exists in the database
                var existingPoll = _context.Polls.FirstOrDefault(p => p.Key == key);
                if (existingPoll == null)
                {
                    // key doesn't exist in the database, return it
                    return key;
                }
            }
        }


        public async Task<ActionResult> OnPostAsync(string question, string[] option, int timeFrame)
        {

            if(!ModelState.IsValid){
                return Page();
            }

            //New instance to save input into the db
            var poll = new Poll
            {
                Question = question,
                Options = option.Select(o => new Option { Text = o }).ToList(),
                Key = GenerateKey(),
                Status = "Open",
                Duration = timeFrame
            };

            // Set the ExpirationTime property of the poll
            poll.SetExpirationTime(DateTime.Now.AddMinutes(timeFrame));
            _context.Polls.Add(poll);
            await _context.SaveChangesAsync();

            // Redirect to the details page for the newly created poll - Details page will be created soon!
            return RedirectToPage("/PollView", new { id = poll.Id.ToString() });

        }
    }
}
