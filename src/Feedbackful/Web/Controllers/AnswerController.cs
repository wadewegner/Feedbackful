using System;
using System.Linq;
using System.Web.Mvc;
using Microsoft.WindowsAzure;
using Web.Entities;
using Web.Utils;

namespace Web.Controllers
{
    public class AnswerController : Controller
    {
        private static readonly string StorageConnectionString = CloudConfigurationManager.GetSetting("StorageConnectionString");
        private readonly Storage _storage = new Storage(StorageConnectionString, Constants.Answers);

        // GET: Answer
        public ActionResult Index()
        {
            var answerEntities = _storage.GetEntities<AnswerEntity>().OrderBy(answers => answers.QuestionKey);
            return View(answerEntities);
        }

        // GET: Answer/Details/5
        public ActionResult Details(string id)
        {
            var answerEntity = _storage.GetEntityByPartionKey<AnswerEntity>(id);

            return View(answerEntity);
        }

        // GET: Answer/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Answer/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                var id = Guid.NewGuid().ToString();
                var questionKey = collection["QuestionKey"];
                var answerEntity = new AnswerEntity(id, questionKey)
                {
                    AnswerKey = id,
                    QuestionKey = questionKey,
                    PossibleAnswer = collection["PossibleAnswer"]
                };

                _storage.InsertEntity(answerEntity);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Answer/Edit/5
        public ActionResult Edit(string id)
        {
            var aswerEntity = _storage.GetEntityByPartionKey<AnswerEntity>(id);

            return View(aswerEntity);
        }

        // POST: Answer/Edit/5
        [HttpPost]
        public ActionResult Edit(string id, FormCollection collection)
        {
            try
            {
                var updatedAnswerEntity = _storage.GetEntityByPartionKey<AnswerEntity>(id);

                updatedAnswerEntity.QuestionKey = collection["QuestionKey"];
                updatedAnswerEntity.PossibleAnswer = collection["PossibleAnswer"];

                _storage.UpdateEntity(updatedAnswerEntity);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Answer/Delete/5
        public ActionResult Delete(string id)
        {
            var answerEntity = _storage.GetEntityByPartionKey<AnswerEntity>(id);

            return View(answerEntity);
        }

        // POST: Answer/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            try
            {
                var answerEntity = _storage.GetEntityByPartionKey<AnswerEntity>(id);
                _storage.DeleteEntity(answerEntity);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}