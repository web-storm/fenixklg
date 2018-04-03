using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using testapp.DataModel;

namespace testapp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult Users(string right = "")
        {
            object result = null;

            if (right == "9d854e76fe0e4a9ba195a93014b8bdf8")
            {
                using (var ctx = new fenixklgEntities())
                {
                    result = ctx.tblUsers.Select(u => new
                    {
                        u.Email,
                        u.Name,
                        u.Surname,
                        u.Nik,
                        u.City,
                        Type = u.tblUserTypes.Name
                    }).ToList();
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Error = "Недостаточно прав!" }, JsonRequestBehavior.AllowGet);
        }
    }
}