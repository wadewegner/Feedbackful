using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage.Table;
using Web.Entities;
using Web.Utils;

namespace Web.Controllers
{
    public class ReportsController : Controller
    {
        private readonly string _storageConnectionString = CloudConfigurationManager.GetSetting("StorageConnectionString");

        // GET: Reports
        public ActionResult Index()
        {
            var questionKey = Request.QueryString["questionKey"];
            var questionCode = Request.QueryString["questionCode"];
            ViewBag.QuestionCode = questionCode;
            ViewBag.QuestionKey = questionKey;
            ViewBag.Question = Request.QueryString["question"];

            var storage = new Storage(_storageConnectionString);
            var answersTable = storage.GetStorageTable("answers");

            IEnumerable<AnswerEntity> query = (from feedback in answersTable.CreateQuery<AnswerEntity>()
                                               where feedback.QuestionKey == questionKey
                                               select feedback);

            ViewBag.DefaultAnswer1 = "";
            ViewBag.DefaultAnswer2 = "";
            ViewBag.DefaultAnswer3 = "";
            ViewBag.DefaultAnswer4 = "";

            var i = 0;
            string[] categories = new string[query.Count()];
            foreach (var answerEntity in query)
            {
                categories[i] = answerEntity.PossibleAnswer;

                if (i == 0)
                {
                    ViewBag.DefaultAnswer1 = answerEntity.PossibleAnswer;
                }
                if (i == 1)
                {
                    ViewBag.DefaultAnswer2 = answerEntity.PossibleAnswer;
                }
                if (i == 2)
                {
                    ViewBag.DefaultAnswer3 = answerEntity.PossibleAnswer;
                }
                if (i == 3)
                {
                    ViewBag.DefaultAnswer4 = answerEntity.PossibleAnswer;
                }

                i++;
            }

            var answersAndFeedbackTable = storage.GetStorageTable("answersandfeedback");

            IEnumerable<AnswersAndFeedbackEntity> query2 = (from feedback in answersAndFeedbackTable.CreateQuery<AnswersAndFeedbackEntity>()
                                                            where feedback.QuestionCode == questionCode
                                                           select feedback);

            

            var answer1 = query2.Count(answerEntity => answerEntity.Answer1);
            var answer2 = query2.Count(answerEntity => answerEntity.Answer2);
            var answer3 = query2.Count(answerEntity => answerEntity.Answer3);
            var answer4 = query2.Count(answerEntity => answerEntity.Answer4);

            ViewBag.Answer1 = answer1;
            ViewBag.Answer2 = answer2;
            ViewBag.Answer3 = answer3;
            ViewBag.Answer4 = answer4;

            var chart = new DotNet.Highcharts.Highcharts("chart")
                .SetXAxis(new XAxis
                {
                    Categories = categories
                })
                .SetSeries(new Series
                {
                    Data = new Data(new object[] { answer1, answer2, answer3, answer4 })
                });

            return View(chart);
        }

    }
}
