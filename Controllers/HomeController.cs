using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebFrontEndCountries.Models;
using System.Text.Json;
using System.Net;
using RestSharp;
using Newtonsoft.Json;

namespace WebFrontEndCountries.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}

		public static string GetValidateToken()
		//public static string PrimeraValidacion(string Usuario, string password, string Token)
		{

			RestClient client = new RestClient("https://localhost:44389/");
			RestRequest request = new RestRequest("api/Countries/GetToken/", Method.GET);

			request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
			request.AddHeader("cache-control", "no-cache");
			request.AddHeader("content-type", "application/x-www-form-urlencoded");
			client.ConfigureWebRequest((r) =>
			{
				r.ServicePoint.Expect100Continue = false;
				r.KeepAlive = true;
			});
			ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
			IRestResponse response = client.Execute(request);

			if (response.IsSuccessful)
			{
				var content = response.Content;
				return content;
			}
			else
			{
				var content = "No";
				return content;
			}
		}


		public IActionResult Index()
		{
			return View("Index");
		}


		public IActionResult Login(string User, string pwd)
		{

			try
			{
				string Token = GetValidateToken();

				Token = Token.Remove(0, 1);

				Token = Token.Remove(Token.Length - 1);

				string urlWillisAPI = String.Empty;

				RestClient client = new RestClient("https://localhost:44389/");
				RestRequest request = new RestRequest("api/Countries/GetCountriesList/", Method.GET);
				request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
				request.AddHeader("cache-control", "no-cache");
				request.AddHeader("content-type", "application/x-www-form-urlencoded");
				request.AddHeader("Authorization", "Bearer " + Token);

				client.ConfigureWebRequest((r) =>
				{
					r.ServicePoint.Expect100Continue = false;
					r.KeepAlive = true;
				});
				ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
				IRestResponse response = client.Execute(request);

				if (response.IsSuccessful)
				{
					var content = response.Content;

					if (content.Contains("Invalid"))
					{

						return PartialView("VistaErrorGenerico");
					}
					else
					{

						content = content.Remove(0, 8);

						content = content.Remove(content.Length - 11);

						var listCountries = JsonConvert.DeserializeObject<List<Countries>>(content);

						var Countries = listCountries.ToList();

						return PartialView("Main", Countries.ToList());
					}
				}
				else
				{
					ViewBag.Error = "IsSuccessful";

					return PartialView("Error");
				}

			}
			catch (Exception ex)
			{
				var Error = ex;

				return PartialView("Error");
			}
		}


		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
