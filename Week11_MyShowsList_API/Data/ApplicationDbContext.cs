using Microsoft.EntityFrameworkCore;
using Week11_MyShowsList_API.Models;

namespace Week11_MyShowsList_API.Data
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
		{

		}

		public DbSet<User> Users { get; set; }
		public DbSet<Show> Shows { get; set; }
		public DbSet<MyShow> MyShows { get; set; }
	}
}
