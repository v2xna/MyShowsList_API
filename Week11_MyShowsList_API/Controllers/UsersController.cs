using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Week11_MyShowsList_API.Data;
using Week11_MyShowsList_API.Models;

namespace Week11_MyShowsList_API.Controllers
{
	[Route("api/[controller]/{action}")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		private readonly ApplicationDbContext _context;

		public UsersController(ApplicationDbContext context)
		{
			_context = context;
		}

		// Get All Users
		[HttpGet]
		public ActionResult<IEnumerable<User>> GetAllUsers()
		{
			var users = _context.Users.ToList();
			if (users == null)
			{
				return NotFound();
			}

			return Ok(new { users });
		}


		// Register
		[HttpPost]
		public ActionResult Register(User user)
		{
			var checkUsername = _context.Users.FirstOrDefault(x => x.Username == user.Username);

			if (checkUsername != null)
			{
				return BadRequest(new { Error_Message = "This Username already exist!" });
			}

			try
			{
				_context.Users.Add(user);
				_context.SaveChanges();
			}
			catch (Exception ex)
			{
				return NotFound(new { Error_Message = ex.Message });
			}

			return Ok(user);
		}

		//Login
		[HttpPost]
		public ActionResult Login(LoginRequest login)
		{
			var checkLogin = _context.Users.FirstOrDefault(x => x.Username == login.username && x.Password == login.password);

			if (checkLogin == null)
			{
				return BadRequest(new { Error_Message = "Invalid username or password!" });
			}

			return Ok(new { UserId = checkLogin.Id, FirstName = checkLogin.FirstName, LastName = checkLogin.LastName, Picture = checkLogin.Picture, Username = checkLogin.Username });
		}

		//[HttpPost]
		//public ActionResult Post([FromBody] IDictionary<string, string> body)
		//{
		//	var username = body["username"];
		//	var password = body["password"];

		//	var login = _context.Users.FirstOrDefault(x => x.Username == username && x.Password == password);
		//	if (login == null)
		//	{
		//		return BadRequest(new { Error_Message = "Invalid username or password!" });
		//	}
		//	return Ok(new { username = login.Username, id = login.Id });
		//}

	}
}
