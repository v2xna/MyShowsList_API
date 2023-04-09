using System.ComponentModel.DataAnnotations;

namespace Week11_MyShowsList_API.Models
{
	public class Show
	{
		[Key]
		public string Id { get; set; }

		[Required]
		public string Picture { get; set; }

		[Required]
		public string Title { get; set; }

		[Required]
		public string Synopsis { get; set; }

		[Required]
		public string Type { get; set; }

		[Required]
		public string Genres { get; set; }

		[Required]
		public int Episodes { get; set; }

		[Required]
		public string Studio { get; set; }

		[Required]
		public DateTime Aired { get; set; }

		[Required]
		public string Language { get; set; }

		public Show()
		{

		}

		public Show(string id, string picture, string title, string synopsis, string type, string genres, int episodes, string studio, DateTime aired, string language)
		{
			Id = id;
			Picture = picture;
			Title = title;
			Synopsis = synopsis;
			Type = type;
			Genres = genres;
			Episodes = episodes;
			Studio = studio;
			Aired = aired;
			Language = language;
		}

	}
}
