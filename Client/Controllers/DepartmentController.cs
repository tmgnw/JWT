using System;
using System.Collections.Generic;
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
    public class DepartmentController : Controller
    {
        public IActionResult Index()
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:44374/api/")
            };
            client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("JWTToken"));

            return View(LoadDepartment());
        }

        public JsonResult LoadDepartment()
        {
            DepartmentJson departmentVM = null;
            var client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:44374/api/")
            };
            var responseTask = client.GetAsync("Department");
            responseTask.Wait();
            var result = responseTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var json = JsonConvert.DeserializeObject(result.Content.ReadAsStringAsync().Result).ToString();
                departmentVM = JsonConvert.DeserializeObject<DepartmentJson>(json);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "server error, try after some time");
            }
            return Json(departmentVM);
        }

        public JsonResult InsertOrUpdate(DepartmentVM departmentVM)
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:44374/api/")
            };
            var myContent = JsonConvert.SerializeObject(departmentVM);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            if (departmentVM.Id.Equals(0))
            {
                var result = client.PostAsync("Department/", byteContent).Result;
                return Json(result);
            }
            else
            {
                var result = client.PutAsync("Department/" + departmentVM.Id, byteContent).Result;
                return Json(result);    
            }
        }

        public JsonResult GetById(int Id)
        {
            DepartmentVM departmentVM = null;
            var client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:44374/api/")
            };
            var responseTask = client.GetAsync("Department/" + Id);
            responseTask.Wait();
            var result = responseTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var json = JsonConvert.DeserializeObject(result.Content.ReadAsStringAsync().Result).ToString();
                departmentVM = JsonConvert.DeserializeObject<DepartmentVM>(json);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "server error, try after some time");
            }
            return Json(departmentVM);
        }

        public JsonResult Delete(int Id)
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:44374/api/")
            };
            var result = client.DeleteAsync("Department/" + Id).Result;
            return Json(result);
        }
    }
}