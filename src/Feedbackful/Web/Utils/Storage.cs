using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace Web.Utils
{
    public class Storage
    {
        private readonly string _storageConnectionString;

        public Storage(string storageConnectionString)
        {
            this._storageConnectionString = storageConnectionString;
        }

        public CloudTable GetStorageTable(string tableName)
        {
            var storageAccount = CloudStorageAccount.Parse(_storageConnectionString);
            var tableClient = storageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference(tableName.ToLower());
            table.CreateIfNotExists();
            return table;
        }
    }
}