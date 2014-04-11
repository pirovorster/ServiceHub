using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace ServiceHub.Website.Controllers
{
    public class TestController : BaseController
    {


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult HomePage()
        { 
            return View(); 
        }
    }
}
