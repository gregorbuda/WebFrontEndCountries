﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
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
using System.Net;
using System.IO;
using Microsoft.AspNetCore.Session;

namespace WebFrontEndCountries.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;


		const string SessionToken = "_Token";
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

		public IActionResult VerSesion()
		{
			return PartialView("HandleSession");
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult UpdateDetailsCountries(string Id, string Name, string AlphaOne, string AlphaTwo, string NumericCode, string Independet)
		{

			Countries countries = new Countries();
			countries.CountryId = Convert.ToInt16(Id);
			countries.NameCountry = Name;
			countries.AlphaOneCountry = AlphaOne;
			countries.AlphaTwoCountry = AlphaTwo;
			countries.NumericCode = NumericCode;
			if(Independet == "Yes")
			{
				countries.Independent = true;
			}
			else
			{
				countries.Independent = false;
			}


			string valor = Send<Countries>("https://localhost:44389/api/Countries/UpdateCountries/", countries, "PUT");

			if (valor.Contains("Correct"))
			{
				//db.SpActualizarUsuarios(Usuario, Password, "", Nombre, Email, Telefono, Fax, Convert.ToInt64(Rol), Convert.ToDecimal(Ente), Convert.ToDecimal(TipoPoliza), Convert.ToInt16(UsuarioId));
				return Json(new { success = "Exitoso", responseText = "" });
			}
			else
			{
				return Json(new { success = "No", responseText = "" });
			}
		}

		public string Send<T>(string url, T objectRequest, string method = "PUT")
		{

			string result = "";
			try
			{
				string Token = "";

				string SesionToken = HttpContext.Session.GetString(SessionToken);

				if (SessionToken == null)
				{
					 Token = GetValidateToken();

					Token = Token.Remove(0, 1);

					Token = Token.Remove(Token.Length - 1);

					HttpContext.Session.SetString(SessionToken, Token);
				}
				else
				{
					Token = HttpContext.Session.GetString(SessionToken);
				}

				//serializamos el objeto
				string json = Newtonsoft.Json.JsonConvert.SerializeObject(objectRequest);

				//peticion
				WebRequest request = WebRequest.Create(url);

				//headers
				request.Method = method;
				request.PreAuthenticate = true;
				request.Headers.Add("Authorization", "Bearer " + Token);
				request.ContentType = "application/json";
				request.Timeout = 10000000; //opcional

				using (var streamWriter = new StreamWriter(request.GetRequestStream()))
				{
					streamWriter.Write(json);
					streamWriter.Flush();
				}
				var httpResponse = (HttpWebResponse)request.GetResponse();
				using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
				{
					result = streamReader.ReadToEnd();
				}
			}
			catch (Exception e)
			{
				result = e.Message;
			}
			return result;
		}

		public IActionResult DetailsCountries(string Id, string Name, string AlphaOne, string AlphaTwo, string NumericCode, string Independet)
		{
			ViewBag.Id = Id;
			ViewBag.Name = Name;
			ViewBag.AlphaOne = AlphaOne;
			ViewBag.AlphaTwo = AlphaTwo;
			ViewBag.NumericCode = NumericCode;
			ViewBag.Independet = Independet;

			return PartialView("DetailsCountries");
		}



		public IActionResult DeleteCountries(string Id, string Name)
		{
			ViewBag.Id = Id;
			ViewBag.Country = Name;

			return PartialView("DeleteCountry");
		}




		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult DeleteCountry(string Id, string Name)
		{

			Countries countries = new Countries();
			countries.CountryId = Convert.ToInt16(Id);
			countries.NameCountry = Name;

			string valor = SendDelete<Countries>("https://localhost:44389/api/Countries/DeleteCountries/", countries, "DELETE");

			if (valor.Contains("Correct"))
			{
				//db.SpActualizarUsuarios(Usuario, Password, "", Nombre, Email, Telefono, Fax, Convert.ToInt64(Rol), Convert.ToDecimal(Ente), Convert.ToDecimal(TipoPoliza), Convert.ToInt16(UsuarioId));
				return Json(new { success = "Exitoso", responseText = "" });
			}
			else
			{
				return Json(new { success = "No", responseText = "" });
			}
		}


		public string SendDelete<T>(string url, T objectRequest, string method = "DELETE")
		{

			string result = "";
			try
			{
				string Token = "";

				string SesionToken = HttpContext.Session.GetString(SessionToken);

				if (SessionToken == null)
				{
					Token = GetValidateToken();

					Token = Token.Remove(0, 1);

					Token = Token.Remove(Token.Length - 1);

					HttpContext.Session.SetString(SessionToken, Token);
				}
				else
				{
					Token = HttpContext.Session.GetString(SessionToken);
				}

				//serializamos el objeto
				string json = Newtonsoft.Json.JsonConvert.SerializeObject(objectRequest);

				//peticion
				WebRequest request = WebRequest.Create(url);

				//headers
				request.Method = method;
				request.PreAuthenticate = true;
				request.Headers.Add("Authorization", "Bearer " + Token);
				request.ContentType = "application/json";
				request.Timeout = 10000000; //opcional

				using (var streamWriter = new StreamWriter(request.GetRequestStream()))
				{
					streamWriter.Write(json);
					streamWriter.Flush();
				}
				var httpResponse = (HttpWebResponse)request.GetResponse();
				using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
				{
					result = streamReader.ReadToEnd();
				}
			}
			catch (Exception e)
			{
				result = e.Message;
			}
			return result;
		}


		public IActionResult ListSubDivision(string Id)
		{
			ViewBag.Id = Id;

			return PartialView("ListSubDivision");
		}


		public IActionResult Login(string User, string pwd)
		{

			try
			{
				string Token = GetValidateToken();

				Token = Token.Remove(0, 1);

				Token = Token.Remove(Token.Length - 1);

				HttpContext.Session.SetString(SessionToken, Token);

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

					return PartialView("Error");
				}

			}
			catch (Exception ex)
			{

				return PartialView("Error");
			}
		}

		public IActionResult CountriesList()
		{

			try
			{
				string Token = "";

				string SesionToken = HttpContext.Session.GetString(SessionToken);

				if (SessionToken == null)
				{
					Token = GetValidateToken();

					Token = Token.Remove(0, 1);

					Token = Token.Remove(Token.Length - 1);

					HttpContext.Session.SetString(SessionToken, Token);
				}
				else
				{
					Token = HttpContext.Session.GetString(SessionToken);
				}


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

					return PartialView("Error");
				}

			}
			catch (Exception ex)
			{

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
