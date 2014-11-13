using Microsoft.AspNet.SignalR;

namespace Web.Hubs
{
    public class FeedbackHub : Hub
    {
        public void Feedback(string presentationCode, string questionCode, string answer1, string answer2, string answer3, string answer4, string feedback)
        {
            Clients.All.feedback(presentationCode, questionCode, answer1, answer2, answer3, answer4, feedback);
        }
    }
}