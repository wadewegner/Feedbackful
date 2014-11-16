using System.Web.Mvc;
using Microsoft.WindowsAzure;
using Web.Entities;
using Web.Utils;

namespace Web.Controllers
{
    public class DisplayAnswersController : Controller
    {
        private readonly string _storageConnectionString = CloudConfigurationManager.GetSetting("StorageConnectionString");

        // GET: DisplayAnswers
        public ActionResult Index()
        {
            var storage = new Storage(_storageConnectionString, Constants.AnswersAndFeedback);
            var answersAndFeedbackEntity = storage.GetEntities<AnswersAndFeedbackEntity>();

            return View(answersAndFeedbackEntity);
        }
    }
}