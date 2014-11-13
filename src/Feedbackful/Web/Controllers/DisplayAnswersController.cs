using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage.Table;
using Web.Entities;
using Web.Utils;

namespace Web.Controllers
{
    public class DisplayAnswersController : Controller
    {
        private readonly string _storageConnectionString = CloudConfigurationManager.GetSetting("StorageConnectionString");
        private const string StorageTable = "answersandfeedback";

        // GET: ApiDefinition
        public ActionResult Index()
        {
            var storage = new Storage(_storageConnectionString);
            var answerEntitiesCloudTable = storage.GetStorageTable(StorageTable);
            var answerEntitiesQuery = new TableQuery<AnswersAndFeedbackEntity>();
            var answerEntities = answerEntitiesCloudTable.ExecuteQuery(answerEntitiesQuery);

            var enumerable = answerEntities as AnswersAndFeedbackEntity[] ?? answerEntities.ToArray();
            if (enumerable.Any())
            {
                return View(enumerable.ToList());
            }

            return View();
        }
    }
}