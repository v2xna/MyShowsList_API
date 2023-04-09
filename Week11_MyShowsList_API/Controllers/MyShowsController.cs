using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Week11_MyShowsList_API.Data;
using Week11_MyShowsList_API.Models;

namespace Week11_MyShowsList_API.Controllers
{
	[Route("api/[controller]/{action}")]
	[ApiController]
	public class MyShowsController : ControllerBase
	{
		private readonly ApplicationDbContext _context;
		public List<Show> Shows = new List<Show>();


		public MyShowsController(ApplicationDbContext context)
		{
			_context = context;
		}

		// Get One
		[HttpGet("{id}")]
		public ActionResult<MyShow> GetOneMyShow(int id)
		{
			// Find() --> will crash database if it doesn't find	|	FirstOrDefault() --> will just return null if it doesn't find
			var myShow = _context.MyShows.FirstOrDefault(x => x.Id == id);

			if (myShow == null)
			{
				// Bad!
				//return NotFound($"Show {id} Not Found!");
				return NotFound(new { Error_Message = $"MyShow {id} Not Found!" });
			}
			return Ok(myShow);
		}

		// Get all Favorite Shows by User
		[HttpGet("{userId}")]
		public ActionResult<IEnumerable<MyShow>> GetShowsByUser(int userId)
		{
			//var myShow = _context.MyShows.FirstOrDefault(x => x.UserId == userId);
			//if (myShow == null)
			//{
			//	return NotFound(new { Error_Message = $"UserId {userId} Not Found!"});
			//}

			var myFavorites = _context.MyShows.Where(x => x.UserId == userId).ToList();

			foreach (var show in myFavorites)
			{
				Shows.Add(_context.Shows.FirstOrDefault(x => x.Id == show.ShowId));
			}

			return Ok(new { myFavorites, Shows });
		}


		// Add a Show to my favorites
		[HttpPost]
		public ActionResult Post(MyShow myShow)
		{
			var findShow = _context.MyShows.FirstOrDefault(x => x.ShowId == myShow.ShowId && x.UserId == myShow.UserId);

			if (findShow != null)
			{
				return BadRequest(new { Error_Message = $"This Show {myShow.ShowId} already exist in your favorites." });
			}

			try
			{
				_context.Add(myShow);
				_context.SaveChanges();
			}
			catch (Exception ex)
			{
				return BadRequest(new {Error_Message = ex.Message});
			}

			return Ok(new { myShow });
		}

		// Update progression of the show
		[HttpPut("{id}")]
		public ActionResult Put(int id, MyShow myShow)
		{
			// Checks if URL id and body id matches
			if (id != myShow.Id)
			{
				return BadRequest(new { Error_Message = "Ids do not match!" });
			}

			var myShowToUpdate = _context.MyShows.FirstOrDefault(x => x.Id == myShow.Id);
			if (myShowToUpdate == null)
			{
				return NotFound(new { Error_Message = $"MyShow {id} Not Found!" });
			}
			_context.Entry(myShowToUpdate).State = EntityState.Detached;

			try
			{
				_context.Update(myShow);
				_context.SaveChanges();
			}
			catch (Exception ex)
			{
				return BadRequest(new { Error_Message = ex.Message });
			}

			return Ok(myShow);
		}

		// Delete
		[HttpDelete("{id}")]
		public ActionResult Delete (int id)
		{
			var myShow = _context.MyShows.FirstOrDefault(x => x.Id == id);

			if (myShow == null)
			{
				return NotFound(new { Error_Message = $"MyShow {id} Not Found!" });
			}

			try
			{
				_context.Remove(myShow);
				_context.SaveChanges();
			}
			catch(Exception ex)
			{
				return BadRequest(new { Error_Message = ex.Message });
			}

			return Ok(myShow);
		}
	}
}
