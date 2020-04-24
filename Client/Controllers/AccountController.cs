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
        private HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:44374/api/")
        };

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginVM loginVM)
        {
            //AccountViewModel accountVM = null;

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

                if (role == "Admin")
                {
                    return RedirectToAction("Index", "Department");
                }
                else
                {
                    return RedirectToAction("AccEmployee", "Account");
                }
            }
            return View();
            //return Json(result);
        }

        public JsonResult LoadEmployee()
        {
            client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("JWTToken"));
            //EmployeeJson employeeVM = null;
            //EmployeeVM employee = null;

            object datas = null;
            var email = HttpContext.Session.GetString("Email");
            var responseTask = client.GetAsync("Employee/" + email);
            responseTask.Wait();
            var result = responseTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var json = JsonConvert.DeserializeObject(result.Content.ReadAsStringAsync().Result).ToString();
                datas = JsonConvert.SerializeObject(json);
            }
            else
            {
                return Json(result);
            }
            return Json(datas);

            //var responseTask = client.GetAsync("Employee/" + email);
            //responseTask.Wait();
            //var result = responseTask.Result;
            //if (result.IsSuccessStatusCode)
            //{
            //    var json = JsonConvert.DeserializeObject(result.Content.ReadAsStringAsync().Result).ToString();
            //    employee = JsonConvert.DeserializeObject<EmployeeVM>(json);
            //}
            //else
            //{
            //    ModelState.AddModelError(string.Empty, "Server Error");
            //}
            //return Json(employee);
        }

        public JsonResult EditEmp(Employee model)
        {
            client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("JWTToken"));
            var myContent = JsonConvert.SerializeObject(model);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var result = client.PutAsync("Employee/" + model.Email, byteContent).Result;
            return Json(result);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Email");
            HttpContext.Session.Remove("JWToken");
            HttpContext.Session.Remove("Role");
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult Not_Found()
        {
            return View();
        }

        public IActionResult AccEmployee()
        {
            var role = HttpContext.Session.GetString("Role");

            if (role == "Employee")
            {
                return View();
            }
            else
            {
                return RedirectToAction("Not_Found", "Account");
            }
            //return View();
        }
    }
}