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
    public class HomeController : Controller
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
    }
}
