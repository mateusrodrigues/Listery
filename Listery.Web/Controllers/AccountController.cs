using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Listery.Web.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account/SignOut
        public void SignOut()
        {
            HttpContext.GetOwinContext().Authentication.SignOut();
        }
    }
}