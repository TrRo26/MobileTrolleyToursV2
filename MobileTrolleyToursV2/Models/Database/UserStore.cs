using System;
using Microsoft.AspNetCore.Identity;
using MobileTrolleyTours.Models.Users;
using MobileTrolleyTours.Data;
using Azure.Data.Tables;
using MobileTrolleyTours.Models.Enums;

namespace MobileTrolleyTours.Models.Database
{
    public class UserStore : IUserStore<User> //, IUserEmailStore<User>, IUserPhoneNumberStore<User>, IUserTwoFactorStore<User>, IUserPasswordStore<User>
    {
        private readonly string _connectionString;
        private readonly User _user;

        public UserStore(IUser user)  //IConfiguration configuration)
        {
            _connectionString = new AzureStorageConfig().ConnectionString;  //configuration.GetConnectionString("DefaultConnection");

            _user = (User?)user;
        }

        public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            TableEntity testEntity = new TableEntity("User", "1")
                {
                    { "TestTest", "TestValue" },
                };

            TableStorageService.AddEntity(testEntity);


            //using (var connection = new SqlConnection(_connectionString))
            //{
            //    await connection.OpenAsync(cancellationToken);
            //    user.Id = await connection.QuerySingleAsync<int>($@"INSERT INTO [User] ([UserName], [NormalizedUserName], [Email],
            //[NormalizedEmail], [EmailConfirmed], [PasswordHash], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled])
            //VALUES (@{nameof(User.UserName)}, @{nameof(User.NormalizedUserName)}, @{nameof(User.Email)},
            //@{nameof(User.NormalizedEmail)}, @{nameof(User.EmailConfirmed)}, @{nameof(User.PasswordHash)},
            //@{nameof(User.PhoneNumber)}, @{nameof(User.PhoneNumberConfirmed)}, @{nameof(User.TwoFactorEnabled)});
            //SELECT CAST(SCOPE_IDENTITY() as int)", user);
            //}

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            //using (var connection = new SqlConnection(_connectionString))
            //{
            //    await connection.OpenAsync(cancellationToken);
            //    await connection.ExecuteAsync($"DELETE FROM [User] WHERE [Id] = @{nameof(User.Id)}", user);
            //}

            return IdentityResult.Success;
        }

        public async Task<User> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            //using (var connection = new SqlConnection(_connectionString))
            //{
            //    await connection.OpenAsync(cancellationToken);
            //    return await connection.QuerySingleOrDefaultAsync<User>($@"SELECT * FROM [User]
            //    WHERE [Id] = @{nameof(userId)}", new { userId });
            //}

            return new User();
        }

        public async Task<User> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            //using (var connection = new SqlConnection(_connectionString))
            //{
            //    await connection.OpenAsync(cancellationToken);
            //    return await connection.QuerySingleOrDefaultAsync<User>($@"SELECT * FROM [User]
            //    WHERE [NormalizedUserName] = @{nameof(normalizedUserName)}", new { normalizedUserName });
            //}

            return new User();
        }

        public Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedUserName);
        }

        public Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id.ToString());
        }

        public Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedUserName = normalizedName;
            return Task.FromResult(0);
        }

        public Task SetUserNameAsync(User user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.FromResult(0);
        }

        public async Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            //using (var connection = new SqlConnection(_connectionString))
            //{
            //    await connection.OpenAsync(cancellationToken);
            //    await connection.ExecuteAsync($@"UPDATE [User] SET
            //    [UserName] = @{nameof(User.UserName)},
            //    [NormalizedUserName] = @{nameof(User.NormalizedUserName)},
            //    [Email] = @{nameof(User.Email)},
            //    [NormalizedEmail] = @{nameof(User.NormalizedEmail)},
            //    [EmailConfirmed] = @{nameof(User.EmailConfirmed)},
            //    [PasswordHash] = @{nameof(User.PasswordHash)},
            //    [PhoneNumber] = @{nameof(User.PhoneNumber)},
            //    [PhoneNumberConfirmed] = @{nameof(User.PhoneNumberConfirmed)},
            //    [TwoFactorEnabled] = @{nameof(User.TwoFactorEnabled)}
            //    WHERE [Id] = @{nameof(User.Id)}", user);
            //}

            return IdentityResult.Success;
        }

        //public Task SetEmailAsync(User user, string email, CancellationToken cancellationToken)
        //{
        //    user.Email = email;
        //    return Task.FromResult(0);
        //}

        //public Task<string> GetEmailAsync(User user, CancellationToken cancellationToken)
        //{
        //    return Task.FromResult(user.Email);
        //}

        //public Task<bool> GetEmailConfirmedAsync(User user, CancellationToken cancellationToken)
        //{
        //    return Task.FromResult(user.EmailConfirmed);
        //}

        //public Task SetEmailConfirmedAsync(User user, bool confirmed, CancellationToken cancellationToken)
        //{
        //    user.EmailConfirmed = confirmed;
        //    return Task.FromResult(0);
        //}

        //public async Task<User> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        //{
        //    cancellationToken.ThrowIfCancellationRequested();

        //    //using (var connection = new SqlConnection(_connectionString))
        //    //{
        //    //    await connection.OpenAsync(cancellationToken);
        //    //    return await connection.QuerySingleOrDefaultAsync<User>($@"SELECT * FROM [User]
        //    //    WHERE [NormalizedEmail] = @{nameof(normalizedEmail)}", new { normalizedEmail });
        //    //}

        //    return new User();
        //}

        //public Task<string> GetNormalizedEmailAsync(User user, CancellationToken cancellationToken)
        //{
        //    return Task.FromResult(user.NormalizedEmail);
        //}

        //public Task SetNormalizedEmailAsync(User user, string normalizedEmail, CancellationToken cancellationToken)
        //{
        //    user.NormalizedEmail = normalizedEmail;
        //    return Task.FromResult(0);
        //}

        //public Task SetPhoneNumberAsync(User user, string phoneNumber, CancellationToken cancellationToken)
        //{
        //    user.PhoneNumber = phoneNumber;
        //    return Task.FromResult(0);
        //}

        //public Task<string> GetPhoneNumberAsync(User user, CancellationToken cancellationToken)
        //{
        //    return Task.FromResult(user.PhoneNumber);
        //}

        //public Task<bool> GetPhoneNumberConfirmedAsync(User user, CancellationToken cancellationToken)
        //{
        //    return Task.FromResult(user.PhoneNumberConfirmed);
        //}

        //public Task SetPhoneNumberConfirmedAsync(User user, bool confirmed, CancellationToken cancellationToken)
        //{
        //    user.PhoneNumberConfirmed = confirmed;
        //    return Task.FromResult(0);
        //}

        //public Task SetTwoFactorEnabledAsync(User user, bool enabled, CancellationToken cancellationToken)
        //{
        //    user.TwoFactorEnabled = enabled;
        //    return Task.FromResult(0);
        //}

        //public Task<bool> GetTwoFactorEnabledAsync(User user, CancellationToken cancellationToken)
        //{
        //    return Task.FromResult(user.TwoFactorEnabled);
        //}

        //public Task SetPasswordHashAsync(User user, string passwordHash, CancellationToken cancellationToken)
        //{
        //    user.PasswordHash = passwordHash;
        //    return Task.FromResult(0);
        //}

        //public Task<string> GetPasswordHashAsync(User user, CancellationToken cancellationToken)
        //{
        //    return Task.FromResult(user.PasswordHash);
        //}

        //public Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken)
        //{
        //    return Task.FromResult(user.PasswordHash != null);
        //}

        public void Dispose()
        {
            // Nothing to dispose.
        }
    }
}




