using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using MobileTrolleyTours.Models.Enums;

namespace MobileTrolleyTours.Models.Users
{
    public class User : IdentityUser, IUser
    {
        public readonly UserDbContext _context;

        public PartitionKeys PartitionKey { get; set; }
        //public int Id { get; set; }
        public int Role { get; set; }
        public string? Password { get; set; }
        //public string UserName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public UserStatus Status { get; set; }

        public User()
        {
            //_context = context;
        }
    }
}

