using System;
using System.Linq;
using System.Web.Mvc;
using Microsoft.WindowsAzure;
using Web.Entities;
using Web.Utils;

namespace Web.Controllers
{
    public class QuestionController : Controller
    {
        private static readonly string StorageConnectionString = CloudConfigurationManager.GetSetting("StorageConnectionString");
        private readonly Storage _storage = new Storage(StorageConnectionString, Constants.Questions);

        // GET: Question
        public ActionResult Index()
        {
            var questionEntities = _storage.GetEntities<QuestionEntity>().OrderBy(question => question.QuestionCode);
            return View(questionEntities);
        }
        
        // GET: Question/Details/5
        public ActionResult Details(string id)
        {
            var questionEntity = _storage.GetEntityByPartionKey<QuestionEntity>(id);
            return View(questionEntity);
        }

        // GET: Question/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Question/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                var id = Guid.NewGuid().ToString();
                var code = collection["PresentationCode"];
                var questionEntity = new QuestionEntity(id, code)
                {
                    QuestionKey = id,
                    PresentationCode = code,
                    QuestionCode = collection["QuestionCode"],
                    Question = collection["Question"],
                };

                _storage.InsertEntity(questionEntity);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Question/Edit/5
        public ActionResult Edit(string id)
        {
            var questionEntity = _storage.GetEntityByPartionKey<QuestionEntity>(id);
            return View(questionEntity);
        }

        // POST: Question/Edit/5
        [HttpPost]
        public ActionResult Edit(string id, FormCollection collection)
        {
            try
            {
                var updatedQuestionEntity = _storage.GetEntityByPartionKey<QuestionEntity>(id);

                updatedQuestionEntity.PresentationCode = collection["PresentationCode"];
                updatedQuestionEntity.QuestionCode = collection["QuestionCode"];
                updatedQuestionEntity.Question = collection["Question"];

                _storage.UpdateEntity(updatedQuestionEntity);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Question/Delete/5
        public ActionResult Delete(string id)
        {
            var questionEntity = _storage.GetEntityByPartionKey<QuestionEntity>(id);

            return View(questionEntity);
        }

        // POST: Question/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            try
            {
                var questionEntity = _storage.GetEntityByPartionKey<QuestionEntity>(id);
                _storage.DeleteEntity(questionEntity);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}