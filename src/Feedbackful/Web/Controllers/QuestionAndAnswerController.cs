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
        private const string QuestionStorageTable = "questions";
        private const string AnswersStorageTable = "answers";

        public QuestionAndAnswers Get([FromUri]string presentationCode, [FromUri]string questionCode)
        {
            var storage = new Storage(_storageConnectionString);
            var questionEntityTable = storage.GetStorageTable(QuestionStorageTable);
            var questionEntityQuery = new TableQuery<QuestionEntity>();
            var questionEntities = questionEntityTable.ExecuteQuery(questionEntityQuery);

            if (questionEntities != null)
            {
                foreach (var questionEntity in questionEntities)
                {
                    if (questionEntity.PresentationCode == presentationCode &&
                        questionEntity.QuestionCode == questionCode)
                    {
                        var answerEntitiesCloudTable = storage.GetStorageTable(AnswersStorageTable);
                        var answerEntitiesQuery = new TableQuery<AnswerEntity>();
                        var answerEntities = answerEntitiesCloudTable.ExecuteQuery(answerEntitiesQuery);
                        var answerEnumerable = answerEntities as AnswerEntity[] ?? answerEntities.ToArray();

                        var answers = new List<Answer>();

                        foreach (var answer in answerEnumerable)
                        {
                            if (answer.QuestionKey == questionEntity.QuestionKey)
                            {
                                answers.Add(new Answer {PossibleAnswer = answer.PossibleAnswer});
                            }
                        }

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

            var storage = new Storage(_storageConnectionString);
            var table = storage.GetStorageTable("answersandfeedback");
            var insertOperation = TableOperation.Insert(asandfEntity);
            table.Execute(insertOperation);

            var feedbackHub = GlobalHost.ConnectionManager.GetHubContext<FeedbackHub>(); 
 
            feedbackHub.Clients.All.feedback(
                asandf.PresentationCode,
                asandf.QuestionCode,
                asandf.Answer1,
                asandf.Answer2,
                asandf.Answer3,
                asandf.Answer4,
                asandf.Feedback); 

        }
    }
}
