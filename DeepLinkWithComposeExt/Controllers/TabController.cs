using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DeepLinkWithComposeExt.Controllers
{
    public class TabController : Controller
    {

        [Route("tab")]
        public ActionResult Tab()
        {
            return View();
        }

        [Route("configure")]
        public ActionResult Configure()
        {
            return View();
        }

        [Route("statictab")]
        public ActionResult StaticTab()
        {
            return View();
        }
    }
}