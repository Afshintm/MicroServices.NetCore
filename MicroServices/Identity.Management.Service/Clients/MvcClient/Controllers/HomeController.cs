using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using System;

namespace MvcClient.Controllers
{
    public class HomeController : Controller
    {
        IConfiguration _configuration;
        public HomeController(IConfiguration configuration) {
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Secure()
        {
            ViewData["Message"] = "Secure page.";

            return View();
        }

        public async Task Logout()
        {
            await HttpContext.SignOutAsync("Cookies");
            await HttpContext.SignOutAsync("oidc");
        }

        public IActionResult Error()
        {
            return View();
        }

        public async Task<IActionResult> CallApiUsingClientCredentials()
        {
            var tokenEndPoint = _configuration.GetSection("AuthenticationServer")["TokenEndPoint"];
            var clientId = _configuration.GetSection("AuthenticationServer").GetSection("Client")["Id"];
            var clientSecret = _configuration.GetSection("AuthenticationServer").GetSection("Client")["Secret"];
            var scope = _configuration.GetSection("AuthenticationServer").GetSection("Client")["Scope1"];

            var apiBaseUrl = _configuration.GetSection("BackendApi")["BaseUrl"];


            var tokenClient = new TokenClient(tokenEndPoint, clientId, clientSecret);
            var tokenResponse = await tokenClient.RequestClientCredentialsAsync(scope);

            var client = new HttpClient();
            client.SetBearerToken(tokenResponse.AccessToken);
            var content = await client.GetStringAsync(new Uri(new Uri(apiBaseUrl), "identity"));

            ViewBag.Json = JArray.Parse(content).ToString();
            return View("Json");
        }

        public async Task<IActionResult> CallApiUsingUserAccessToken()
        {
            var apiBaseUrl = _configuration.GetSection("BackendApi")["BaseUrl"];

            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var client = new HttpClient();
            client.SetBearerToken(accessToken);
            var content = await client.GetStringAsync(new Uri(new Uri(apiBaseUrl), "identity"));

            ViewBag.Json = JArray.Parse(content).ToString();
            return View("Json");
        }
    }
}