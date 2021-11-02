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
    }
}
