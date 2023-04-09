using System.ComponentModel.DataAnnotations;

namespace Week11_MyShowsList_API.Models
{
	public class User
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[Display(Name = "First Name")]
		public string FirstName { get; set; }

		[Required]
		[Display(Name = "Last Name")]
		public string LastName { get; set; }

		[Required]
		public string Picture { get; set; }

		[Required]
		public string Username { get; set; }

		[Required]
		public string Password { get; set; }

		public User()
		{

		}

		public User(int id, string firstName, string lastName, string picture, string username, string password)
		{
			Id = id;
			FirstName = firstName;
			LastName = lastName;
			Picture = picture;
			Username = username;
			Password = password;
		}
	}
}
