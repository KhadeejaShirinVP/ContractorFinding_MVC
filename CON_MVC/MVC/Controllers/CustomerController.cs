using Domain;
using javax.xml.crypto;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using Newtonsoft.Json;

namespace MVC.Controllers
{
    public class CustomerController : Controller
    {

        private readonly HttpClient client;
        private CrudStatus status;

        private readonly IHttpClientFactory _clientFactory;
        public const string SessionKey = "RegistrationNo";

        string url = "";

        public CustomerController(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            status = new CrudStatus();
            _clientFactory = clientFactory;
            url = "https://localhost:7294/api/";
            client = _clientFactory.CreateClient(".");
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public string? GetRegistrationNo(string sessionkey)
        {
            var registrationNo = HttpContext.Session.GetString(sessionkey);
            return registrationNo;
        }

        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                IEnumerable<CustomerGetModule> customers = new List<CustomerGetModule>();
                HttpResponseMessage responseMessage = client.GetAsync(url + "Customer").Result;
                if (responseMessage.IsSuccessStatusCode)
                {
                    string data = responseMessage.Content.ReadAsStringAsync().Result;
                    customers = JsonConvert.DeserializeObject<List<CustomerGetModule>>(data)!;
                }
                return View(customers);
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        public ActionResult AddCustomerDetails()
        {
            return View();
        }

        [HttpPost]
        [ActionName("AddCustomerDetails")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddCustomerDetails(TbCustomer customer)
        {
            try
            {
                TbCustomer tbCustomer=new TbCustomer();
                CrudStatus crudStatus=new CrudStatus();
                HttpResponseMessage responseMessage=client.PostAsJsonAsync(url+ "Customer/AddCustomerDetails" ,customer).Result;
                string data = responseMessage.Content.ReadAsStringAsync().Result;
                status = JsonConvert.DeserializeObject<CrudStatus>(data)!;
                if (responseMessage.IsSuccessStatusCode)
                {
                    if (status.Status == false)
                    {
                        ModelState.AddModelError(string.Empty, status.Message!);
                        return View(customer);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Customer");
                    }
                }
                ModelState.AddModelError(string.Empty, "Server Error");
                return View(customer);
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        public ActionResult UpdateCustomerDetails()
        {
            return View();
        }
        [HttpPost]
        [ActionName("UpdateCustomerDetails")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateCustomerDetails(TbCustomer updateDetails)
        {
            try
            {
                TbCustomer customer = new TbCustomer();
                CrudStatus crudStatus = new CrudStatus();
                HttpResponseMessage responseMessage = client.PutAsJsonAsync(url + "Customer/UpdateDetails", updateDetails).Result;
                string data = responseMessage.Content.ReadAsStringAsync().Result;
                status = JsonConvert.DeserializeObject<CrudStatus>(data)!;
                if (responseMessage.IsSuccessStatusCode)
                {
                    if (status.Status == false)
                    {
                        ModelState.AddModelError(string.Empty, status.Message!);
                        return View(updateDetails);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Customer");
                    }
                }
                ModelState.AddModelError(string.Empty, "Server Error");
                return View(updateDetails);
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        public ActionResult DeleteCustomer(string RegistrationNo)
        {
            HttpContext.Session.SetString(SessionKey, RegistrationNo!);
            return View();
        }
        [HttpPost]
        //[ActionName("DeleteCustomer")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCustomer(string RegistrationNo,TbCustomer customer)
        {
            try
            {
                RegistrationNo = HttpContext.Session.GetString(SessionKey);
                //TbCustomer cus = new TbCustomer();
                var data = client.DeleteAsync(url + "Customer/DeleteCustomer?RegistrationNo=" + RegistrationNo);
                data.Wait();
                var result = data.Result;
                var resultMessage = result.Content.ReadAsStringAsync().Result;
                status = JsonConvert.DeserializeObject<CrudStatus>(resultMessage);
                if (result.IsSuccessStatusCode)
                {
                    if (status.Status == true)
                    {
                        ModelState.AddModelError(string.Empty, status.Message);
                        return RedirectToAction("Index", "Customer");
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
