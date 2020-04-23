using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using API.Models;
using API.ViewModels;
using Microsoft.AspNetCore.Http;
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
            //var role = HttpContext.Session.GetString("Role");
            //if (role == "Employee")
            //{
            //    return View(LoadEmployee());
            //}
            //return RedirectToAction("Index","Account");
            return View(LoadEmployee());
        }

        public JsonResult LoadEmployee()
        {
            //client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("JWTToken"));
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

        public JsonResult Insert(RegisterVM employee)
        {
            var myContent = JsonConvert.SerializeObject(employee);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var result = client.PostAsync("Auth/register", byteContent).Result;
            return Json(result);
        }

        public JsonResult Updatee(EmployeeVM employeeVM)
        {
            var myContent = JsonConvert.SerializeObject(employeeVM);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var result = client.PutAsync("Employee/" + employeeVM.Email, byteContent).Result;
            return Json(result);
        }

        public JsonResult GetById(string Email)
        {
            EmployeeVM employee = null;
            var responseTask = client.GetAsync("Employee/" + Email);
            responseTask.Wait();
            var result = responseTask.Result;
            if (result.IsSuccessStatusCode)
            {
                //var readTask = result.Content.ReadAsAsync<IList<EmployeeVM>>();
                //readTask.Wait();
                //employee = readTask.Result;
                var json = JsonConvert.DeserializeObject(result.Content.ReadAsStringAsync().Result).ToString(); 
                employee = JsonConvert.DeserializeObject<EmployeeVM>(json);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Server Error");
            }
            return Json(employee);
        }

        public JsonResult Delete(string Email)
        {
            var result = client.DeleteAsync("Employee/" + Email).Result;
            return Json(result);
        }

        //public JsonResult GetDonut()
        //{
        //    IEnumerable<ChartVM> chartInfo = null;
        //    List<ChartVM> chartData = new List<ChartVM>();
            
        //    //client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("JWToken"));
        //    var responseTask = client.GetAsync("Employee/ChartInfo"); //Access data from employees API
        //    responseTask.Wait(); //Waits for the Task to complete execution.
        //    var result = responseTask.Result;
        //    if (result.IsSuccessStatusCode) // if access success
        //    {
        //        var readTask = result.Content.ReadAsAsync<IList<ChartVM>>(); //Get all the data from the API
        //        readTask.Wait();
        //        chartInfo = readTask.Result;
        //        foreach (var item in chartInfo)
        //        {
        //            ChartVM data = new ChartVM();
        //            data.label = item.label;
        //            data.value = item.value;
        //            chartData.Add(data);
        //        }
        //        var json = JsonConvert.SerializeObject(chartData, Formatting.Indented);
        //        return Json(json);
        //    }
        //    return Json("internal server error");
        //}

        public JsonResult GetChart()
        {
            IEnumerable<ChartVM> chartInfo = null;
            List<ChartVM> chartData = new List<ChartVM>();
            
            //client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("JWToken"));
            var responseTask = client.GetAsync("Employee/ChartInfo"); //Access data from employees API
            responseTask.Wait(); //Waits for the Task to complete execution.
            var result = responseTask.Result;
            if (result.IsSuccessStatusCode) // if access success
            {
                var readTask = result.Content.ReadAsAsync<IList<ChartVM>>(); //Get all the data from the API
                readTask.Wait();
                chartInfo = readTask.Result;
                foreach (var item in chartInfo)
                {
                    ChartVM data = new ChartVM();
                    data.label = item.label;
                    data.value = item.value;
                    chartData.Add(data);
                }
                var json = JsonConvert.SerializeObject(chartData, Formatting.Indented);
                return Json(json);
            }
            return Json("internal server error");
        }
    }
}