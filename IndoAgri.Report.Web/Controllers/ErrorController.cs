using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IndoAgri.Report.Web.Controllers
{
    public class ErrorController : Controller
    {
        public ViewResult Index(string message)
        {
            ViewData["message"] = message;
            return View("Error");
        }
        public ViewResult NotFound()
        {
            Response.StatusCode = 404;  //you may want to set this to 200
            return View("NotFound");
        }
    }
}