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
using java.beans;

namespace MVC.Controllers
{
    public class ContractorController : Controller
    {
        private readonly HttpClient client;
        private CrudStatus status;

        private readonly IHttpClientFactory _clientFactory;
        public const string SessionKey = "License";

        string url = "";

        public ContractorController(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            status = new CrudStatus();
            _clientFactory = clientFactory;
            url = "https://localhost:7294/api/";
            client = _clientFactory.CreateClient(".");

        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public string? GetLicense(string sessionkey)
        {
            var license = HttpContext.Session.GetString(sessionkey);
            return license;
        }

        //Get Contractor
        [HttpGet]
        public ActionResult Index(int pin)
        {
            try
            {
                IEnumerable<ContractorGetModule> contractors = new List<ContractorGetModule>();
                if (pin == 0)
                {
                    HttpResponseMessage responseMessage = client.GetAsync(url + "Contractor").Result;
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        string data = responseMessage.Content.ReadAsStringAsync().Result;
                        contractors = JsonConvert.DeserializeObject<List<ContractorGetModule>>(data)!;
                    }
                    return View(contractors);
                }
                else
                {
                    HttpResponseMessage responseMessage = client.GetAsync(url + "Customer/Pincode?pin="+pin).Result;
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        string data = responseMessage.Content.ReadAsStringAsync().Result;
                        contractors = JsonConvert.DeserializeObject<List<ContractorGetModule>>(data)!;
                    }
                    return View(contractors);
                }
               
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }
    
        //Add Details
        public ActionResult AddingDetails()
        {
            return View();
        }

        [HttpPost]
        [ActionName("AddingDetails")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddingDetails(ContractorDetail contractorDetail)
        {
            try
            {
                CrudStatus crudStatus=new CrudStatus();
                ContractorDetail contractor= new ContractorDetail();
                HttpResponseMessage responseMessage = client.PutAsJsonAsync(url + "Contractor/CreateContractor", contractorDetail).Result;
                string data = responseMessage.Content.ReadAsStringAsync().Result;
                status = JsonConvert.DeserializeObject<CrudStatus>(data)!;
                if (responseMessage.IsSuccessStatusCode)
                {
                    if (status.Status == false)
                    {
                        ModelState.AddModelError(string.Empty, status.Message!);
                        return View(contractorDetail);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Contractor");
                    }
                }
                ModelState.AddModelError(string.Empty, "Server Error");
                return View(contractorDetail);
               
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        //update Contractor Details
        public ActionResult UpdateDetails()
        {
            return View();
        }
        [HttpPost]
        [ActionName("UpdateDetails")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateDetails(ContractorDetail updateDetails)
        {
            try
            {
                ContractorDetail contractor=new ContractorDetail();
                CrudStatus crudStatus=new CrudStatus();
                HttpResponseMessage responseMessage = client.PutAsJsonAsync(url + "Contractor/UpdateDetails", updateDetails).Result;
                string data=responseMessage.Content.ReadAsStringAsync().Result;
                status = JsonConvert.DeserializeObject<CrudStatus>(data)!;
                if(responseMessage.IsSuccessStatusCode)
                {
                    if (status.Status == false)
                    {
                        ModelState.AddModelError(string.Empty, status.Message!); 
                        return View(updateDetails);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Contractor");
                    }
                }
                ModelState.AddModelError(string.Empty, "Server Error");
                return View(updateDetails);
            }
            catch(Exception ex)
            {
                return View(ex.Message);
            }
        }

        public ActionResult DeleteContractor(string License)
        {
            HttpContext.Session.SetString(SessionKey, License!);
            return View();
        }
        [HttpPost]
        //[ActionName("DeleteContractor")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteContractor(string License, ContractorDetail contractor)
        {
            try
            {
                 License = HttpContext.Session.GetString(SessionKey);
                ContractorDetail con = new ContractorDetail();
                //contractor.License = License;
                var data = client.DeleteAsync(url + "Contractor/DeleteContractor?License=" + License);
                data.Wait();
                var result = data.Result;
                var resultMessage = result.Content.ReadAsStringAsync().Result;
                status = JsonConvert.DeserializeObject<CrudStatus>(resultMessage);
                if (result.IsSuccessStatusCode)
                {
                    if (status.Status == true)
                    {
                        ModelState.AddModelError(string.Empty, status.Message);
                        return RedirectToAction("Index", "Contractor");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, status.Message!);
                        return View();
                    }
                }
                ModelState.AddModelError(string.Empty, "Server Error");
                return View();
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }
    }
}
