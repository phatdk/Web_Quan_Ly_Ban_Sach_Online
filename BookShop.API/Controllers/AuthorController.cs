using BookShop.BLL.ConfigurationModel.AuthorModel;
using BookShop.BLL.ConfigurationModel.UserModel;
using BookShop.BLL.IService;
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
		public async Task<List<AuthorModel>> Get(string? page)
		{
			if (page != null)
			{
				int pageIndex = int.Parse(page);

				return (await _service.Getall()).Skip((pageIndex - 1) * 2).Take(2).ToList();
			}else return await _service.Getall();
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
