using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace Web.Utils
{
    public class Storage
    {
        public readonly CloudTable CloudTable;

        public Storage(string storageConnectionString, string tableName)
        {
            try
            { 
                var account = CloudStorageAccount.Parse(storageConnectionString);
                var tableClient = account.CreateCloudTableClient();
                
                CloudTable = tableClient.GetTableReference(tableName.ToLower());
                CloudTable.CreateIfNotExists();
            }
            catch (StorageException ex)
            {
                Trace.TraceError(ex.ToString());
            }
        }

        public List<T> GetEntities<T>() where T : TableEntity, new()
        {
            try
            {
                var exQuery = new TableQuery<T>();
                var results = CloudTable.ExecuteQuery(exQuery).Select(ent => ent).ToList();
                
                return results;
            }
            catch (StorageException ex)
            {
                Trace.TraceError(ex.ToString());
                return null;
            }
        }

        public T GetEntityByPartionKey<T>(string partitionKey) where T : TableEntity, new()
        {
            try
            {
                var exQuery = new TableQuery<T>().Where(
                    TableQuery.GenerateFilterCondition(
                        "PartitionKey",
                        QueryComparisons.Equal,
                        partitionKey));
                
                var results = CloudTable.ExecuteQuery(exQuery).FirstOrDefault();
                return results;
            }
            catch (StorageException ex)
            {
                Trace.TraceError(ex.ToString());
                return null;
            }
        }

        public void InsertEntity<T>(T entity) where T : TableEntity
        {
            try
            {
                var insertOperation = TableOperation.Insert(entity);
                CloudTable.Execute(insertOperation);
            }
            catch (StorageException ex)
            {
                Trace.TraceError(ex.ToString());
            }
        }

        public void UpdateEntity<T>(T entity) where T : TableEntity
        {
            try 
            { 
                var updateOperation = TableOperation.Replace(entity);
                CloudTable.Execute(updateOperation);
            }
            catch (StorageException ex)
            {
                Trace.TraceError(ex.ToString());
            }
        }

        public void DeleteEntity<T>(T entity) where T : TableEntity
        {
            try 
            { 
                var deleteOperation = TableOperation.Delete(entity);
                CloudTable.Execute(deleteOperation);
            }
            catch (StorageException ex)
            {
                Trace.TraceError(ex.ToString());
            }
        }
    }
}