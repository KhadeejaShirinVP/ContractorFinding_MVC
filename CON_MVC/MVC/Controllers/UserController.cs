using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using System.Net.Http;
using Domain;
using System.Net.Http.Json;
using javax.security.auth.spi;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using javax.xml.crypto;
//using sun.net.www.http;

namespace MVC.Controllers
{
    public class UserController : Controller
    {

        private readonly HttpClient client;
        private CrudStatus status;
        private readonly IHttpClientFactory _clientFactory;     
        string url = "";

        public UserController(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            status = new CrudStatus();
            _clientFactory = clientFactory;
            url = "https://localhost:7294/api/";
            client = _clientFactory.CreateClient(".");

        }

        //Getting User Details
        [HttpGet]
        public async Task<IActionResult> IndexAsync()
        {
            try
            {
                IEnumerable<GetUser> getUsers = new List<GetUser>();
                var result = await client.GetAsync(url + "User");
                if (result.IsSuccessStatusCode)
                {
                    var readJob = result.Content.ReadFromJsonAsync<IList<GetUser>>();
                    readJob.Wait();
                    getUsers = readJob.Result!;
                }
                return View(getUsers);
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        //For User Registration
        public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Registration")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Registration(Registration registration)
        {
            try
            {
                CrudStatus crudStatus = new CrudStatus();
                // HttpResponseMessage responseMessage= client.PostAsJsonAsync(url+"User", registration).Result;
                var result = await client.PostAsJsonAsync<Registration>(url + "User", registration);
                string data = result.Content.ReadAsStringAsync().Result;
                registration = JsonConvert.DeserializeObject<Registration>(data)!;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("UserLogin", "User");
                }
                ModelState.AddModelError(string.Empty, "Server Error");
                return View(registration);
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        //For User Login
        public ActionResult UserLogin()
        {
            return View();
        }

        [HttpPost]
        [ActionName("UserLogin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UserLogin(Login login)
        {
            try
            {
                Login log = new Login();
                CrudStatus crudStatus = new CrudStatus();
                HttpResponseMessage responseMessage = client.PostAsJsonAsync(url + "User/LoginUser", login).Result;
                string result = responseMessage.Content.ReadAsStringAsync().Result;
                status = JsonConvert.DeserializeObject<CrudStatus>(result)!;
                if (responseMessage.IsSuccessStatusCode)
                {
                    if (status.Status == false)
                    {
                        ModelState.AddModelError(string.Empty, status.Message!);
                        return View(login);
                    }
                    return RedirectToAction("Index", "User");
                }
                ModelState.AddModelError(string.Empty, "server Error");
                return View(login);
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        //For Updating password
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ActionName("ForgotPassword")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(Login forgotPassword)
        {
            try
            {
                Login userFogotPassword = new Login();
                CrudStatus crudStatus = new CrudStatus();
                HttpResponseMessage responseMessage = client.PutAsJsonAsync(url + "User/ForgotPassword", forgotPassword).Result;
                string data = responseMessage.Content.ReadAsStringAsync().Result;
                status = JsonConvert.DeserializeObject<CrudStatus>(data)!;
                if (responseMessage.IsSuccessStatusCode)
                {
                    if (status.Status == false)
                    {
                        ModelState.AddModelError(string.Empty, status.Message!);
                        return View(forgotPassword);
                    }
                    else
                    {
                        return RedirectToAction("UserLogin", "User");
                    }
                }
                ModelState.AddModelError(string.Empty, "Server Error");
                return View(forgotPassword);
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }
    }
}