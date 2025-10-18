using DeployOnAzure.Repository;
using infrastructure.Dto.UserManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DeployOnAzure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository repository)
        {
            _userRepository = repository;
        }

        [HttpGet()]
        public async Task<ActionResult> GetAll()
        {
            var Results = _userRepository.GetAllAsync();
            return Ok(Results);
        }

        [HttpGet("GetById/{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var user = new UserDto { Id = 1, FirstName ="Azadeh", LastName="Dehghan", Active=true, LastLoginDate=DateTime.Now, CreateDate=DateTime.Now, NationalCode="1234567890" };
            return Ok(user);
        }
    }
}
