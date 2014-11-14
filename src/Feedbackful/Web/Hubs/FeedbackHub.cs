using Microsoft.AspNet.SignalR;

namespace Web.Hubs
{
    public class FeedbackHub : Hub
    {
        public void Feedback(string presentationCode, string questionCode, int answer1, int answer2, int answer3, int answer4, string feedback)
        {
            Clients.All.feedback(
                new FeedbackBit
                {
                    PresentationCode = presentationCode,
                    QuestionCode = questionCode,
                    Answer1 = answer1,
                    Answer2 = answer2,
                    Answer3 = answer3,
                    Answer4 = answer4, 
                    Feedback = feedback
                });
        }

        public override System.Threading.Tasks.Task OnConnected()
        {
            return base.OnConnected();
        }
    }

    public class FeedbackBit
    {
        public string PresentationCode { get; set; }
        public string QuestionCode { get; set; }
        public int Answer1 { get; set; }
        public int Answer2 { get; set; }
        public int Answer3 { get; set; }
        public int Answer4 { get; set; }
        public string Feedback { get; set; }
    }
}