using System;
using Azure.Data.Tables;
using MobileTrolleyTours.Models;
using MobileTrolleyTours.Models.Enums;
using MobileTrolleyTours.Models.Users;

namespace MobileTrolleyTours.Data
{
	public class UserService
	{
        public AzureStorageConfig storageConfig { get; set; }

        public UserService()
		{
            var storageConfig = new AzureStorageConfig();
            TableStorageService.Initialize(storageConfig.TableUsers);
        }

        public static List<User> GetAllUsers()
        {
            var users = new List<User>();
            var entities = TableStorageService.GetEntitiesByPartitionKey(PartitionKeys.User.ToString());

            foreach (TableEntity entity in entities)
            {
                entity.TryGetValue("PartitionKey", out object pKey);
                entity.TryGetValue("RowKey", out object userId);
                entity.TryGetValue("Role", out object role);
                entity.TryGetValue("PasswordHash", out object passwordHash);
                entity.TryGetValue("UserName", out object userName);
                entity.TryGetValue("FirstName", out object firstName);
                entity.TryGetValue("LastName", out object lastName);
                entity.TryGetValue("Status", out object status);


                Enum.TryParse((string?)pKey, out PartitionKeys partKey);

                var user = new User
                {
                    PartitionKey = partKey,
                    Id = userId.ToString(),
                    Role = Int32.Parse(role.ToString()),
                    PasswordHash = passwordHash.ToString(),
                    UserName = userName.ToString(),
                    FirstName = firstName.ToString(),
                    LastName = lastName.ToString(),
                    Status = (UserStatus)Int32.Parse(status.ToString())

                };

                if (user.Status == UserStatus.Active)
                {
                    users.Add(user);
                }
            }

            return users;
        }
    }
}

