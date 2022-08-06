using EmployeeManagement.Data;
using EmployeeManagement.Entities;
using EmployeeManagement.Helpers;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        #region properties
        private IRepository<User> _usersRepo;
        private AppSettings _appSettings;
        #endregion

        #region CTOR
        public UsersController(IRepository<User> usersRepo, AppSettings appSettings)
        {
            _usersRepo = usersRepo;
            _appSettings = appSettings;
        }
        #endregion

        #region Public methods 
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate(AuthenticateRequest model)
        {
            var response = await AuthenticateUser(model);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(response);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _usersRepo.GetAll();
            return Ok(users);
        }

        #endregion

        #region Private methods 
        private async Task<AuthenticateResponse> AuthenticateUser(AuthenticateRequest model)
        {
            var users = await _usersRepo.GetAll(); 
            var user = users.ToList().SingleOrDefault(x => x.Username == model.Username && x.Password == model.Password);

            if (user == null) return null;

            var token = generateJwtToken(user);

            return new AuthenticateResponse(user, token);
        }

        private string generateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor); 
            return tokenHandler.WriteToken(token);
        }

        #endregion
    }
}