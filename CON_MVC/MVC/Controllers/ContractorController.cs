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
namespace MVC.Controllers
{
    public class ContractorController : Controller
    {
        private readonly HttpClient client;
        private CrudStatus status;

        private readonly IHttpClientFactory _clientFactory;

        string url = "";

        public ContractorController(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            status = new CrudStatus();
            _clientFactory = clientFactory;
            url = "https://localhost:7294/api/";
            client = _clientFactory.CreateClient(".");

        }

        //Get Contractor
        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                IEnumerable<ContractorGetModule> contractors = new List<ContractorGetModule>();
                HttpResponseMessage responseMessage = client.GetAsync(url + "Contractor").Result;
                if (responseMessage.IsSuccessStatusCode)
                {
                    string data = responseMessage.Content.ReadAsStringAsync().Result;
                    contractors = JsonConvert.DeserializeObject<List<ContractorGetModule>>(data)!;
                }
                return View(contractors);
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
                //HttpResponseMessage responseMessage= await client.PutAsJsonAsync<ContractorDetail>(url+ "Contractor/CreateContractor").Result;
                //string result=responseMessage.Content.ReadAsStringAsync().Result;
                var result = await client.PutAsJsonAsync<ContractorDetail>(url + "Contractor/CreateContractor", contractorDetail);
                string data = result.Content.ReadAsStringAsync().Result;
                contractorDetail = JsonConvert.DeserializeObject<ContractorDetail>(data)!;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Contractor");
                }
                ModelState.AddModelError(string.Empty, status.Message);
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

    }
}
