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
    public class AnswerController : Controller
    {
        private readonly string _storageConnectionString = CloudConfigurationManager.GetSetting("StorageConnectionString");
        private const string StorageTable = "answers";
       
        // GET: ApiDefinition
        public ActionResult Index()
        {
            var storage = new Storage(_storageConnectionString);
            var answerEntitiesCloudTable = storage.GetStorageTable(StorageTable);
            var answerEntitiesQuery = new TableQuery<AnswerEntity>();
            var answerEntities = answerEntitiesCloudTable.ExecuteQuery(answerEntitiesQuery);

            var enumerable = answerEntities as AnswerEntity[] ?? answerEntities.ToArray();
            if (enumerable.Any())
            {
                return View(enumerable.ToList());
            }

            return View();
        }

        // GET: ApiDefinition/Details/5
        public ActionResult Details(string id)
        {
            var answerEntity = GetAnswerEntityById(id);

            return View(answerEntity);
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
                var id = Guid.NewGuid().ToString();
                var questionKey = collection["QuestionKey"];
                var questionAndAnswer = new AnswerEntity(id, questionKey)
                {
                    AnswerKey = id,
                    QuestionKey = questionKey,
                    PossibleAnswer = collection["PossibleAnswer"]
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
            var aswerEntity = GetAnswerEntityById(id);

            return View(aswerEntity);
        }

        // POST: ApiDefinition/Edit/5
        [HttpPost]
        public ActionResult Edit(string id, FormCollection collection)
        {
            try
            {
                var updatedAnswerEntity = GetAnswerEntityById(id);

                updatedAnswerEntity.QuestionKey = collection["QuestionKey"];
                updatedAnswerEntity.PossibleAnswer = collection["PossibleAnswer"];

                var storage = new Storage(_storageConnectionString);
                var table = storage.GetStorageTable(StorageTable);
                var updateOperation = TableOperation.Replace(updatedAnswerEntity);
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
            var answerEntity = GetAnswerEntityById(id);

            return View(answerEntity);
        }

        // POST: ApiDefinition/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            try
            {
                var questionAndAnswerEntity = GetAnswerEntityById(id);

                var storage = new Storage(_storageConnectionString);
                var table = storage.GetStorageTable(StorageTable);
                var deleteOperation = TableOperation.Delete(questionAndAnswerEntity);

                table.Execute(deleteOperation);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        private AnswerEntity GetAnswerEntityById(string id)
        {
            var storage = new Storage(_storageConnectionString);
            var answerEntityTable = storage.GetStorageTable(StorageTable);
            var answerEntityQuery = new TableQuery<AnswerEntity>().Where(
                TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, id));
            var answerEntity = answerEntityTable.ExecuteQuery(answerEntityQuery).FirstOrDefault();
            return answerEntity;
        }
    }
}