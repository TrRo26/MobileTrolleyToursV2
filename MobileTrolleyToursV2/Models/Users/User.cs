using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MobileTrolleyTours.Models.Users
{
    public class User : IdentityUser, IUser
    {
        public readonly UserDbContext _context;

        //public int Id { get; set; }
        public int Role { get; set; }
        public string? Password { get; set; }
        //public string UserName { get; set; }
        //public string NormalizedUserName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public User()
        {
            //_context = context;
        }
    }
}

