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
    public class QuestionController : Controller
    {
        private readonly string _storageConnectionString = CloudConfigurationManager.GetSetting("StorageConnectionString");
        private const string StorageTable = "questions";

        // GET: ApiDefinition
        public ActionResult Index()
        {
            var storage = new Storage(_storageConnectionString);
            var apiDefinitionEntitiesCloudTable = storage.GetStorageTable(StorageTable);
            var apiDefinitionEntitiesQuery = new TableQuery<QuestionEntity>();
            var apiDefinitionEntities = apiDefinitionEntitiesCloudTable.ExecuteQuery(apiDefinitionEntitiesQuery);

            var enumerable = apiDefinitionEntities as QuestionEntity[] ?? apiDefinitionEntities.ToArray();
            if (enumerable.Any())
            {
                return View(enumerable.ToList());
            }

            return View();
        }

        // GET: ApiDefinition/Details/5
        public ActionResult Details(string id)
        {
            var questionEntity = GetQuestionEntityById(id);

            return View(questionEntity);
        }

        // GET: ApiDefinition/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ApiDefinition/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here
                var id = Guid.NewGuid().ToString();
                var code = collection["PresentationCode"];
                var questionAndAnswer = new QuestionEntity(id, code)
                {
                    QuestionKey = id,
                    PresentationCode = code,
                    QuestionCode = collection["QuestionCode"],
                    Question = collection["Question"],
                };

                var storage = new Storage(_storageConnectionString);
                var table = storage.GetStorageTable(StorageTable);
                var insertOperation = TableOperation.Insert(questionAndAnswer);
                table.Execute(insertOperation);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: ApiDefinition/Edit/5
        public ActionResult Edit(string id)
        {
            var questionEntity = GetQuestionEntityById(id);

            return View(questionEntity);
        }

        // POST: ApiDefinition/Edit/5
        [HttpPost]
        public ActionResult Edit(string id, FormCollection collection)
        {
            try
            {
                var updatedQuestionEntity = GetQuestionEntityById(id);

                updatedQuestionEntity.PresentationCode = collection["PresentationCode"];
                updatedQuestionEntity.QuestionCode = collection["QuestionCode"];
                updatedQuestionEntity.Question = collection["Question"];

                var storage = new Storage(_storageConnectionString);
                var table = storage.GetStorageTable(StorageTable);
                var updateOperation = TableOperation.Replace(updatedQuestionEntity);
                table.Execute(updateOperation);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: ApiDefinition/Delete/5
        public ActionResult Delete(string id)
        {
            var questionEntity = GetQuestionEntityById(id);

            return View(questionEntity);
        }

        // POST: ApiDefinition/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            try
            {
                var questionEntity = GetQuestionEntityById(id);

                var storage = new Storage(_storageConnectionString);
                var table = storage.GetStorageTable(StorageTable);
                var deleteOperation = TableOperation.Delete(questionEntity);

                table.Execute(deleteOperation);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        private QuestionEntity GetQuestionEntityById(string id)
        {
            var storage = new Storage(_storageConnectionString);
            var questionEntityTable = storage.GetStorageTable(StorageTable);
            var questionEntityQuery = new TableQuery<QuestionEntity>().Where(
                TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, id));
            var questionEntity = questionEntityTable.ExecuteQuery(questionEntityQuery).FirstOrDefault();
            return questionEntity;
        }
    }
}