using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage.Table;

namespace Web.Entities
{
    public class AnswersAndFeedbackEntity : TableEntity
    {
        public AnswersAndFeedbackEntity(string id, string name)
        {
            this.PartitionKey = id;
            this.RowKey = name;
        }

        public AnswersAndFeedbackEntity() { }
        
        public string PresentationCode { get; set; }
        public string QuestionCode { get; set; }
        public bool Answer1 { get; set; }
        public bool Answer2 { get; set; }
        public bool Answer3 { get; set; }
        public bool Answer4 { get; set; }
        public string Feedback { get; set; }
    }
}