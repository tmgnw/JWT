using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Client.Controllers
{
    public class EmployeeController : Controller
    {
        readonly HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:44374/api/")
        };

        public IActionResult Index()
        {
            return View(LoadEmployee());
        }

        public JsonResult LoadEmployee()
        {
            EmployeeJson employeeVM = null;
            var responseTask = client.GetAsync("Employee");
            responseTask.Wait();
            var result = responseTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var json = JsonConvert.DeserializeObject(result.Content.ReadAsStringAsync().Result).ToString();
                employeeVM = JsonConvert.DeserializeObject<EmployeeJson>(json);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "server error, try after some time");
            }
            return Json(employeeVM);
        }

        //public JsonResult InsertOrUpdate(EmployeeVM employeeVM)
        //{
        //    var myContent = JsonConvert.SerializeObject(employeeVM);
        //    var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
        //    var byteContent = new ByteArrayContent(buffer);
        //    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        //    if (employeeVM.Id.Equals(0))
        //    {
        //        var result = client.PostAsync("Employee/", byteContent).Result;
        //        return Json(result);
        //    }
        //    else
        //    {
        //        var result = client.PutAsync("Employee/" + employeeVM.Id, byteContent).Result;
        //        return Json(result);
        //    }
        //}

        public JsonResult GetById(int Id)
        {
            IEnumerable<EmployeeVM> employee = null;
            var responseTask = client.GetAsync("Employee/" + Id);
            responseTask.Wait();
            var result = responseTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<IList<EmployeeVM>>();
                readTask.Wait();
                employee = readTask.Result;
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Server Error");
            }
            return Json(employee);
        }

        public JsonResult Delete(int Id)
        {
            var result = client.DeleteAsync("Employee/" + Id).Result;
            return Json(result);
        }
    }
}