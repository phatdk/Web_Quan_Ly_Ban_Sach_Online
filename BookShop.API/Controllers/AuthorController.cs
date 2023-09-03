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
		public ICollectionService _collectionService;
		public IUserService _userService;
		public AuthorController(IAuthorService service, ICollectionService collectionService, IUserService userService)
		{
			_service = service;
			_collectionService = collectionService;
			_userService = userService;
		}
		[HttpGet("collection")]
		public async Task<IActionResult> Getallss()
		{
			var ok = await _collectionService.Getall();
			return Ok(ok);
		}
		// GET api/<AuthorController>/5
		[HttpGet("{id}")]
		public string Get(int id)
		{
			return "value";
		}
		[HttpPost("add/user")]
		public async Task<IActionResult> add([FromBody] CreateUserModel model)
		{
			var obj = await _userService.add(model);
		 return Ok(obj);
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

	
	}
}
