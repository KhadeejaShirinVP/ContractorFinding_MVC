using Domain;
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

        string url = "";

        public CustomerController(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            status = new CrudStatus();
            _clientFactory = clientFactory;
            url = "https://localhost:7294/api/";
            client = _clientFactory.CreateClient(".");
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
    }
       
    
}
