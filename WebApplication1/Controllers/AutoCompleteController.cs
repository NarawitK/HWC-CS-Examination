using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class AutoCompleteController : Controller
    {
        private HREntities db = new HREntities();
        // GET: AutoComplete
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult GetBossName(string key)
        {
            var name = (from c in db.HR_Employee
                        where c.EmployeeID == key
                        select new { name = c.FirstName + " " + c.LastName, id = c.EmployeeID, }).Distinct();
            return Json(name.ToList(), JsonRequestBehavior.AllowGet);
        }
            
    }
}