using System.Linq;
using System.Web.Mvc;
using Microsoft.WindowsAzure;
using Web.Entities;
using Web.Utils;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly string _storageConnectionString = CloudConfigurationManager.GetSetting("StorageConnectionString");

        // GET: Home
        public ActionResult Index()
        {
            var storage = new Storage(_storageConnectionString, Constants.Questions);
            var questionEntities = storage.GetEntities<QuestionEntity>().OrderBy(question => question.QuestionCode);

            return View(questionEntities);
        }
    }
}
