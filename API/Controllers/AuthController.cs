using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using API.Models;
using API.Repository.Data;
using API.ViewModels;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers
{ 
    //[Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly EmployeeRepository _employeeRepository;

        DynamicParameters parameters = new DynamicParameters();

        public AuthController(IConfiguration configuration, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager, EmployeeRepository employeeRepository)
        {
            _configuration = configuration;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _employeeRepository = employeeRepository;
        }

        // /register
        [Route("register")]
        [HttpPost]
        public async Task<ActionResult> InsertUser([FromBody] RegisterVM model)
        {
            var user = new IdentityUser
            {
                Email = model.Email,
                UserName = model.Email,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _employeeRepository.Create(model);
                await _userManager.AddToRoleAsync(user, "Employee");
                
            }
            return Ok(new { Email = user.Email });
        }

        [Route("login")] // /login
        [HttpPost]
        public async Task<ActionResult> Login([FromBody] LoginVM model)
        {

            var user = await _userManager.FindByNameAsync(model.Email);
            using (var connection = new SqlConnection(_configuration.GetConnectionString("MyNetCoreConnection")))
            {
                var procName = "SP_GetRole";
                parameters.Add("@Email", user.Email);
                IEnumerable<LoginVM> data = connection.Query<LoginVM>(procName, parameters, commandType: CommandType.StoredProcedure);
                foreach (LoginVM users in data)
                {
                    model.Role = users.Role;
                }
            }

            //var user = await _userManager.FindByNameAsync(model.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var claim = new[] {
                    new Claim("Email", user.Email),
                    new Claim("Role", model.Role)
                };
                var signinKey = new SymmetricSecurityKey(
                  Encoding.UTF8.GetBytes(_configuration["Jwt:SigningKey"]));

                int expiryInMinutes = Convert.ToInt32(_configuration["Jwt:ExpiryInMinutes"]);

                var token = new JwtSecurityToken(
                  _configuration["Jwt:Issuer"],
                  _configuration["Jwt:Audience"],
                  claim,
                  expires: DateTime.UtcNow.AddMinutes(expiryInMinutes),
                  signingCredentials: new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256)
                );

                return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                  //new
                  //{
                  //    token = new JwtSecurityTokenHandler().WriteToken(token),
                  //    expiration = token.ValidTo
                  //});
            }
            return Unauthorized();
        }
    }
}