using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Microsoft.AspNet.SignalR;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage.Table;
using Web.Entities;
using Web.Hubs;
using Web.Models;
using Web.Utils;

namespace Web.Controllers
{
    public class QuestionAndAnswerController : ApiController
    {
        private readonly string _storageConnectionString = CloudConfigurationManager.GetSetting("StorageConnectionString");

        public QuestionAndAnswers Get([FromUri]string presentationCode, [FromUri]string questionCode)
        {
            var questionStorage = new Storage(_storageConnectionString, Constants.Questions);
            var questionEntityQuery = new TableQuery<QuestionEntity>();
            var questionEntities = questionStorage.CloudTable.ExecuteQuery(questionEntityQuery);

            if (questionEntities != null)
            {
                foreach (var questionEntity in questionEntities)
                {
                    if (questionEntity.PresentationCode == presentationCode &&
                        questionEntity.QuestionCode == questionCode)
                    {
                        var answerStorage = new Storage(_storageConnectionString, Constants.Answers);
                        var answerEntities = answerStorage.GetEntities<AnswerEntity>();
                        var answers = (from answer in answerEntities 
                                       where answer.QuestionKey == questionEntity.QuestionKey 
                                       select new Answer {PossibleAnswer = answer.PossibleAnswer}).ToList();

                        var qanda = new QuestionAndAnswers
                        {
                            Question = questionEntity.Question,
                            Answers = answers
                        };

                        return qanda;
                    }
                }
            }
            return null;
        }

        public void Post(
            AnswersAndFeedback asandf)
        {
            var id = Guid.NewGuid().ToString();
            
            var asandfEntity = new AnswersAndFeedbackEntity(id, asandf.PresentationCode)
            {
                PresentationCode = asandf.PresentationCode,
                QuestionCode = asandf.QuestionCode,
                Answer1 = asandf.Answer1,
                Answer2 = asandf.Answer2,
                Answer3 = asandf.Answer3,
                Answer4 = asandf.Answer4,
                Feedback = asandf.Feedback,
            };

            var storage = new Storage(_storageConnectionString, Constants.AnswersAndFeedback);
            storage.InsertEntity(asandfEntity);

            IEnumerable<AnswersAndFeedbackEntity> query = ( from feedback in storage.CloudTable.CreateQuery<AnswersAndFeedbackEntity>()
                                                            where feedback.QuestionCode == asandf.QuestionCode
                                                            select feedback);

            var answer1 = query.Count(answerEntity => answerEntity.Answer1);
            var answer2 = query.Count(answerEntity => answerEntity.Answer2);
            var answer3 = query.Count(answerEntity => answerEntity.Answer3);
            var answer4 = query.Count(answerEntity => answerEntity.Answer4);

            var feedbackHub = GlobalHost.ConnectionManager.GetHubContext<FeedbackHub>(); 
 
            feedbackHub.Clients.All.feedback(
                new FeedbackBit
                {
                    PresentationCode = asandf.PresentationCode,
                    QuestionCode = asandf.QuestionCode,
                    Answer1 = answer1,
                    Answer2 = answer2,
                    Answer3 = answer3,
                    Answer4 = answer4,
                    Feedback = asandf.Feedback
                });

        }
    }
}
