using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOVSPlugin
{
    public class SOPost
    {
        public List<string> Tags { get; set; }
        public bool IsAnswered { get; set; }
        public int ViewCount { get; set; }
        public int AnswerCount { get; set; }
        public int Score { get; set; }
        public long CreationDate { get; set; }
        public int QuestionID { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }
        public string CreationDateString { get; set; }

        public void FormatDate()
        {
            DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime date = start.AddSeconds(CreationDate).ToLocalTime();

            CreationDateString = date.Date.ToShortDateString();
        }
    }
}
