using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage.Table;

namespace Web.Entities
{
    public class AnswerEntity : TableEntity
    {
        public AnswerEntity(string id, string name)
        {
            this.PartitionKey = id;
            this.RowKey = name;
        }

        public AnswerEntity() { }

        public string AnswerKey { get; set; }
        public string QuestionKey { get; set; }
        public string PossibleAnswer { get; set; }
    }
}