using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Client.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {   
            return View();
        }

        [HttpPost]
        public JsonResult Login(LoginVM loginVM)
        {
            //AccountViewModel accountVM = null;

            var client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:44374/api/")
            };

            var myContent = JsonConvert.SerializeObject(loginVM);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var result = client.PostAsync("Auth/Login/", byteContent).Result;

            if (result.IsSuccessStatusCode)
            {
                var data = result.Content.ReadAsStringAsync().Result;
                var handler = new JwtSecurityTokenHandler();
                var datajson = handler.ReadJwtToken(data);

                // get token
                string token = "Bearer " + data;
                string role = datajson.Claims.First(claim => claim.Type == "Role").Value;
                string email = datajson.Claims.First(claim => claim.Type == "Email").Value;

                //set token
                HttpContext.Session.SetString("JWTToken", token);
                HttpContext.Session.SetString("Role", role);
                HttpContext.Session.SetString("Email", email);

                //if (role == "Admin")
                //{
                //    return RedirectToAction("Index", "Department");
                //}
                //else
                //{
                //    return RedirectToAction("Index", "Employee");
                //}

            }
            //return View();
            return Json(result);
        }
    }
}