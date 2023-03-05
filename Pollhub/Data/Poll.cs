using System.ComponentModel.DataAnnotations.Schema;

namespace Pollhub.Data
{
    public class Poll
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public List<Option> Options { get; set; } // store options as a JSON string
        public string Key { get; set; }
        public int Duration { get; set; }//stores the timeframe set by the poll creator
        public List<VoteIPs> VoteIPs { get; set; } //stores votes as a Json string
        public DateTime ExpirationTime{ get; private set; }
        public string Status { get; set; }
        public void SetExpirationTime (DateTime expirationTime)
        {
            ExpirationTime =  expirationTime;
        }

    }
}
