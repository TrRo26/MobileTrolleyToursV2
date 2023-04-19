using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Net.NetworkInformation;
using Azure;
using Azure.Data.Tables;
using Microsoft.AspNetCore.Http;
using MobileTrolleyTours.Models;
using MobileTrolleyTours.Models.Enums;

namespace MobileTrolleyTours.Data
{
	public static class ScheduleAlertService
	{
        static ScheduleAlertService()
		{
            var storageConfig = new AzureStorageConfig();
            TableStorageService.Initialize(storageConfig.TableTourScheduleChanges);
        }

        //#region Public Methods

        public static ScheduleChangeData GetAlertBoxHeader()
        {
            var alertBoxHeader = GetScheduleChangeData(PartitionKeys.AlertBoxHeader, ScheduleAlertStatus.Active);

            if (alertBoxHeader.Any())
            {
                return alertBoxHeader.FirstOrDefault();
            }

            return new ScheduleChangeData();
        }

        public static ScheduleChangeData GetAlertBoxSubHeader()
        {
            var alertBoxSubHeader = GetScheduleChangeData(PartitionKeys.AlertBoxSubHeader, ScheduleAlertStatus.Active);

            if (alertBoxSubHeader.Any())
            {
                return alertBoxSubHeader.FirstOrDefault();
            }

            return new ScheduleChangeData();
        }

        public static List<ScheduleChangeData> GetActiveAlerts(PartitionKeys partitionKey = PartitionKeys.AlertBoxItem)
        {
            var alerts = GetScheduleChangeData(partitionKey, ScheduleAlertStatus.Active);

            if (alerts.Any())
            {
                return alerts;
            }

            return new List<ScheduleChangeData>();
        }

        public static ScheduleChangeData GetAlertBoxFooter()
        {
            var alertBoxFooter = GetScheduleChangeData(PartitionKeys.AlertBoxFooter, ScheduleAlertStatus.Active);

            if (alertBoxFooter.Any())
            {
                return alertBoxFooter.FirstOrDefault();
            }

            return new ScheduleChangeData();
        }

        public static IOrderedEnumerable<ScheduleChangeData> GetAllAlerts(PartitionKeys partitionKey)
        {
            var alerts = GetScheduleChangeData(partitionKey);
            var sortedAlerts = alerts.OrderByDescending(d => d.StartDate);

            return sortedAlerts;
        }

        public static string AddAlert(PartitionKeys partitionKey, ScheduleChangeData changeData)
        {
            var alertKey = AddScheduleChangeData(partitionKey, changeData);

            return alertKey;
        }

        public static bool UpdateAlertStatus(ScheduleChangeData changeData)
        {
            var result = false;

            try
            {
                result = UpdateScheduleChangeDataStatus(changeData);
            }
            catch (Exception)
            {
                return false;
            }

            return result;
        }

        public static Dictionary<string, int> UpdateAllStatusByPartitionKey(PartitionKeys partitionKey, ScheduleAlertStatus status)
        {
            var tableEntity = new TableEntity(partitionKey.ToString(), null)
            {
                { "Status", (int)status },
            };

            var updatedItems = TableStorageService.BulkUpdateEntitiesByPartitionKey(tableEntity);

            return updatedItems;
        }

        //#endregion

        //#region Private Methods

        private static List<ScheduleChangeData> GetScheduleChangeData(PartitionKeys partitionKey, ScheduleAlertStatus? alertStatus = null)
        {
            var alerts = new List<ScheduleChangeData>();
            var entities = TableStorageService.GetEntitiesByPartitionKey(partitionKey.ToString());

            foreach (TableEntity entity in entities)
            {
                entity.TryGetValue("PartitionKey", out object pKey);
                entity.TryGetValue("RowKey", out object alertId);
                entity.TryGetValue("StartDate", out object startDate);
                entity.TryGetValue("EndDate", out object endDate);
                entity.TryGetValue("Description", out object description);
                entity.TryGetValue("ApplyDate", out object applyDate);
                entity.TryGetValue("RevokeDate", out object revokeDate);
                entity.TryGetValue("Status", out object status);

                Enum.TryParse((string?)pKey, out PartitionKeys partKey);

                var alert = new ScheduleChangeData
                {
                    PartitionKey = partKey,
                    AlertId = new Guid(alertId.ToString()),
                    StartDate = startDate != null ? (DateTimeOffset)startDate : null,
                    EndDate = endDate != null ? (DateTimeOffset)endDate : null,
                    Description = description != null ? (string)description : String.Empty,
                    ApplyDate = applyDate != null ? (DateTimeOffset)applyDate: null,
                    RevokeDate = revokeDate != null ? (DateTimeOffset)revokeDate: null,
                    Status = (ScheduleAlertStatus)status
                };

                if ((alertStatus == null || alert.Status == alertStatus) && alert.Status != ScheduleAlertStatus.Deleted)
                {
                    alerts.Add(alert);
                }
            }

            AutoUpdateStatus(alerts);

            return alerts;
        }

        private static string AddScheduleChangeData(PartitionKeys partitionKey, ScheduleChangeData changeData)
        {
            var rowKey = Guid.NewGuid().ToString();

            try
            {
                TableEntity entity = new TableEntity(partitionKey.ToString(), rowKey)
                {
                    { nameof(changeData.StartDate), changeData.StartDate },
                    { nameof(changeData.EndDate), changeData.EndDate },
                    { nameof(changeData.Description), changeData.Description },
                    { nameof(changeData.ApplyDate), changeData.ApplyDate },
                    { nameof(changeData.RevokeDate), changeData.RevokeDate },
                    { nameof(changeData.Status), (int)changeData.Status },
                };

                TableStorageService.AddEntity(entity);

                Console.WriteLine($"Table entity with partition key {partitionKey.ToString()} " +
                                  $"and row key {rowKey} was added successfully.");
            }
            catch (Exception)
            {
                return Guid.Empty.ToString();
            }

            return rowKey;
        }

        // UPDATE TO ACCEPT ALL PROPS
        private static bool UpdateScheduleChangeDataStatus(ScheduleChangeData changeData)
        {
            var rowKey = changeData.AlertId.ToString();
            var partitionKey = changeData.PartitionKey.ToString();

            TableEntity entity = new TableEntity(partitionKey, rowKey)
            {
                { nameof(changeData.Status), (int)changeData.Status },
            };

            try
            {
                TableStorageService.UpdateEntityByRowKey(rowKey, entity);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        //private TableEntity MapScheduleChangeDataToTableEntity(ScheduleChangeData changeData)
        //{
        //    TableEntity entity = new TableEntity(partitionKey.ToString(), rowKey)
        //        {
        //            { nameof(changeData.StartDate), changeData.StartDate },
        //            { nameof(changeData.EndDate), changeData.EndDate },
        //            { nameof(changeData.Description), changeData.Description },
        //            { nameof(changeData.ApplyDate), changeData.ApplyDate },
        //            { nameof(changeData.RevokeDate), changeData.RevokeDate },
        //            { nameof(changeData.Status), (int)changeData.Status },
        //        };

        //    return entity;
        //}

        // TO-DO: Need to add "ref"?
        private static void AutoUpdateStatus(List<ScheduleChangeData> alerts)
        {
            var currentDate = DateTime.Now.AddDays(1);

            var alertsToUpdate = alerts.Where(a => (a.ApplyDate != null &&
                                                   (a.ApplyDate <= DateTime.Now && a.Status == ScheduleAlertStatus.Pending ||
                                                    a.RevokeDate <= DateTime.Now && a.Status == ScheduleAlertStatus.Active)) ||
                                                    (a.EndDate == null && a.StartDate <= currentDate && a.Status == ScheduleAlertStatus.Active) ||
                                                    (a.EndDate != null && a.EndDate <= currentDate && a.Status == ScheduleAlertStatus.Active));

            foreach (var alert in alertsToUpdate)
            {

                // If alert has EndDate, set it to inactive the day after that date
                // If alert doesn't have an EndDate, set to inactive the day after StartDate
                if (alert.EndDate == null)
                {
                    if (alert.StartDate <= currentDate.AddDays(1))
                    {
                        alert.Status = ScheduleAlertStatus.Inactive;
                    }
                }
                else if (alert.EndDate <= currentDate.AddDays(1))
                {
                    alert.Status = ScheduleAlertStatus.Inactive;
                }
                else
                {
                    alert.Status = alert.Status == ScheduleAlertStatus.Pending ?
                               ScheduleAlertStatus.Active :
                               ScheduleAlertStatus.Inactive;
                }

                try
                {
                    UpdateScheduleChangeDataStatus(alert);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"There was an error while attempting to update " +
                        $"the Status for rowKey {alert.AlertId} of status {alert.Status}. Message: {ex.Message}");
                }
            }
        }

        //#endregion
    }
}
