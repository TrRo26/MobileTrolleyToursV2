using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;

namespace MobileTrolleyTours.Models
{
    public class AzureStorageConfig
    {
        public string StorageAccountName
        {
            get
            {
                return "mobilettstorage";
            }
        }

        public string ConnectionString
        {
            get
            {
                return "DefaultEndpointsProtocol=https;AccountName=mobilettstorage;" +
                       "AccountKey=CGM/TQcARqZtngGv4UUeVjoMF7BS/X8GysXXmEniS/WQicd2" +
                       "FqfhjIvDoCZspsE9AhS9zuHPkW7r+AStRjSTHw==;" +
                       "EndpointSuffix=core.windows.net";
            }
        }

        public string TableTourScheduleChanges
        {
            get
            {
                return "TourScheduleChanges";
            }
        }
    }
}
