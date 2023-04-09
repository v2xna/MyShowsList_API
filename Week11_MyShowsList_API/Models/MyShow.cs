using System.ComponentModel.DataAnnotations;

namespace Week11_MyShowsList_API.Models
{
	public class MyShow
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public int UserId { get; set; }

		[Required]
		public string ShowId { get; set; }

		[Range(0, 10, ErrorMessage = "Rating must be between 0 and 10.")]
		public int Rating { get; set; }

		[Required]
		public string Progress { get; set; }

		[StringLength(100, ErrorMessage = "Comment must be 100 characters or less.")]
		public string Comment { get; set; }

		public MyShow()
		{

		}

	}
}
