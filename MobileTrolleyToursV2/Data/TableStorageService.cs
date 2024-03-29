﻿using System;
using Azure;
using Azure.Data.Tables;
using System.Linq;
using MobileTrolleyTours.Models;
using MobileTrolleyTours.Models.Enums;
using static System.Net.WebRequestMethods;

namespace MobileTrolleyTours.Data
{
	public static class TableStorageService
	{
        private static AzureStorageConfig _storageConfig;
        private static TableClient _tableClient;

        static TableStorageService()
		{
            _storageConfig = new AzureStorageConfig();
		}

        public static bool Initialize(string tableName)
        {
            _tableClient = new TableClient(_storageConfig.ConnectionString, tableName);

            return true;
        }

        public static TableEntity GetEntityByPartitionAndRowKey(string partitionKey, string rowKey)
        {
            var entity = _tableClient.GetEntity<TableEntity>(partitionKey, rowKey);

            return entity.Value;
        }

        public static TableEntity GetEntityByRowKey(string rowKey)
        {
            var entity = _tableClient.Query<TableEntity>(e => e.RowKey == rowKey);

            return entity.FirstOrDefault();
        }

        public static Pageable<TableEntity> GetEntitiesByPartitionKey(string partitionKey)
        {
            var entities = _tableClient.Query<TableEntity>(e => e.PartitionKey == partitionKey);

            return entities;
        }

        public static string AddEntity(TableEntity newEntity)
        {
            Response? addResponse = null;
            string? rowKey = null;

            try
            {
                addResponse = _tableClient.AddEntity(newEntity);
                rowKey = newEntity.RowKey;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return rowKey;
        }

        public static int UpdateEntityByRowKey(string rowKey, TableEntity newEntity)
        {
            var existingEntity = GetEntityByRowKey(rowKey);
            Response? updateResponse = null;

            try
            {
                updateResponse = _tableClient.UpdateEntity(newEntity, existingEntity.ETag);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return updateResponse.Status;
        }

        public static Dictionary<string, int> BulkUpdateEntitiesByPartitionKey(TableEntity data)
        {
            var entityResults = new Dictionary<string, int>();
            var entitiesToUpdate = GetEntitiesByPartitionKey(data.PartitionKey);

            foreach (var entity in entitiesToUpdate)
            {
                data.RowKey = entity.RowKey;
                var updateResponse = _tableClient.UpdateEntity(data, entity.ETag);
                entityResults.Add(entity.RowKey, updateResponse.Status);
            }

            return entityResults;
        }

        public static int DeleteEntityByRowKey(string rowKey)
        {
            var existingEntity = GetEntityByRowKey(rowKey);
            Response? deleteResponse = null;

            try
            {
                deleteResponse = _tableClient.DeleteEntity(existingEntity.PartitionKey, rowKey);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return deleteResponse.Status;
        }
    }
}
