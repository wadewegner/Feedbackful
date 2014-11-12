using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage.Table;

namespace Web.Entities
{
    public class QuestionEntity : TableEntity
    {
        public QuestionEntity(string id, string name)
        {
            this.PartitionKey = id;
            this.RowKey = name;
        }

        public QuestionEntity() { }

        public string QuestionKey { get; set; }
        public string PresentationCode { get; set; }
        public string QuestionCode { get; set; }
        public string Question { get; set; }
    }
}