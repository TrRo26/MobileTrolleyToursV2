using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MobileTrolleyTours.Models.Users
{
	public class UserDbContext : DbContext //IdentityDbContext<User> //IdentityDbContext
    {
		public UserDbContext()
		{
		}
	}
}

