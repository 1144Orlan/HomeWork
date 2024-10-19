using Microsoft.AspNetCore.Mvc;
using WebMVCR1.Models;

namespace WebMVCR1.Controllers
{
	public class HomeController : Controller
	{
		//public IActionResult Index()
		//{
		//	return View();
		//}

		public string Index(string hel) 
		{
			//int hour = DateTime.Now.Hour; 
			//string Greeting = hour < 12 ? "Доброе утро" : "Добрый день" + ", " + hel; 
			string Greeting = ModelClass.ModelHello() + ", " + hel;
			return Greeting; 
		}
	}
}
