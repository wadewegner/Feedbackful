using System.Collections.Generic;

namespace Web.Models
{
    public class QuestionAndAnswers
    {
        public string Question { get; set; }
        public List<Answer> Answers { get; set; }
    }
}