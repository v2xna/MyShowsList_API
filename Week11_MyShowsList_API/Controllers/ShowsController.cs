using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Week11_MyShowsList_API.Data;
using Week11_MyShowsList_API.Models;

namespace Week11_MyShowsList_API.Controllers
{
	// Example: http;//localhost:7060/api/Shows/
	[Route("api/[controller]/{action}")]
	[ApiController]
	public class ShowsController : ControllerBase
	{
		private readonly ApplicationDbContext _context;
		public ShowsController(ApplicationDbContext context)
		{
			_context = context;
		}

		// Http verbs ONLY EXIST to make the application more readable. You can use ANY verbs to make delete,update,fetch info, etc..

		/*
		   - Fetch all the Shows
		   - If you want to return a list in the API --> Use the interface IEnumerable<>
		*/
		[HttpGet]
		public ActionResult<IEnumerable<Show>> Get()
		{
			var shows = _context.Shows.ToList();

			// Give response to user if this specific TABLE (shows) doesn't exist in the database
			if(shows == null)
			{
				/*
					- NotFound() is not of type IEnumerable<Show>
					- this will allow to return shows and NotFound()  -->  ActionResult<IEnumerable<Show>>
					- Everytime you return in API you need to return an OBJECT!
				*/
				return NotFound(new { Error_Message = $"The table doesn't exist anymore"});
			}

			// Must return JSON/object.. NOT ONE array/list. --> Ok() will allow to return an object
			return Ok(new { shows });
		}



		/* 
			- Get One Show (NEED ID)
			- Not returning array or list where I can iterate. ONLY returning single show  -->  ActionResult<Show>
			- Can use the same Get() cuz we're overloading the method so it won't cause conflict
			
		*/
		[HttpGet("{id}", Name = "GetShow")]
		public ActionResult<Show> Get(string id)
		{
			// Find() --> will crash database if it doesn't find	|	FirstOrDefault() --> will just return null if it doesn't find
			var show = _context.Shows.FirstOrDefault(x => x.Id == id);
			
			if(show == null)
			{
				// Bad!
				//return NotFound($"Show {id} Not Found!");
				return NotFound(new { Error_Message = $"Show {id} Not Found!" });
			}
			return Ok(show);
		}


		/*
			- Add A Show
			- To use POST method need to receive 1 Show Object from the user (like a form, receive all the inputs)
		*/
		[HttpPost]
		public ActionResult Post(Show show) // Receive a Request from the body (All the properties).
		{
			var findShow = _context.Shows.FirstOrDefault(x => x.Id == show.Id);
			// if id already exist
			if(findShow != null)
			{
				return BadRequest(new { Error_Message = "This ID already exists!" });
			}
			
			// Prevents app from crashing if a show exist already in the database
			try
			{
				_context.Shows.Add(show);
				_context.SaveChanges();
			}
			catch(Exception ex)
			{
				return BadRequest(new {Error_Message = ex.Message});
			}

			/* 
				- new CreatedAtRouteResult() ---> Returns Object From Database
				- Instead of having to repeat the code to check if the object exist in the database we can reuse the previous function
				- Must provide a 'Name = ""' to the controller in order for this to work
				- needs id for the route parameter --> will only return id
				- add third parameter to see the rest of the object
				- 201 reponse -> good
			*/
			return new CreatedAtRouteResult("GetShow", new {id = show.Id}, show);
		}

		
		// Delete (needs id)
		[HttpDelete("{id}")]
		public ActionResult Delete(string id)
		{
			var show = _context.Shows.FirstOrDefault(x => x.Id == id);

			if (show == null)
			{
				return NotFound(new { Error_Message = $"Show {id} Not Found!" });
			}

			var myShows = _context.MyShows.Where(x => x.ShowId == id).ToList();

			try
			{
				// Main Show Table
				_context.Remove(show);

				// User WatchList Table
				foreach(var item in myShows)
				{
					_context.Remove(item);
				}

				_context.SaveChanges();
			}
			catch(Exception ex)
			{
				// if incase table was deleted in the database
				return BadRequest(new { Error_Message = "Error has occured while saving!" });
			}

			// Response to user
			return Ok(new {show, deleted_From_User_WatchList = myShows});
		}

		
		[HttpPut("{id}")]
		public ActionResult Put(string id, Show show)
		{
			// Checks if URL id and body id matches
			if (id != show.Id)
			{
				return BadRequest(new { Error_Message = "Id's do not match!" });
			}

			var showToUpdate = _context.Shows.FirstOrDefault(x => x.Id == show.Id);
			if(showToUpdate == null)
			{
				return NotFound(new { Error_Message = $"Show {id} Not Found!" });
			}
			_context.Entry(showToUpdate).State = EntityState.Detached; // detached from the memory

			try
			{
				_context.Shows.Update(show);
				_context.SaveChanges();
			}
			catch (Exception ex)
			{
				return BadRequest(new { Error_Message = "Error while trying to save!" });
			}

			return Ok(show);
		}
		
	}
}
