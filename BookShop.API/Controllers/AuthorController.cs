using BLL.ConfigurationModel.UserModel;
using BLL.IService.IAuthorService;
using BookShop.DAL.Entities;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookShop.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthorController : ControllerBase
	{
		public IAuthorService _service;

		public AuthorController(IAuthorService service)
		{
			_service = service;
		}

		// GET: api/<AuthorController>
		[HttpGet]
		public IEnumerable<string> Get()
		{
			return new string[] { "value1", "value2" };
		}

		// GET api/<AuthorController>/5
		[HttpGet("{id}")]
		public string Get(int id)
		{
			return "value";
		}

		// POST api/<AuthorController>
		[HttpPost]
		public async Task<IActionResult> Post([FromBody] CreateAuthorModel value)
		{
			var result = await _service.Add(value);
			if (result)
			{
				return Ok(result);
			}
			else return BadRequest();
		}

		// PUT api/<AuthorController>/5
		[HttpPut("{id}")]
		public void Put(int id, [FromBody] string value)
		{
		}

		// DELETE api/<AuthorController>/5
		[HttpDelete("{id}")]
		public void Delete(int id)
		{
		}
	}
}
