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
        [HttpPost]
        public ActionResult GetBossName(string term)
        {
            var name = (from c in db.HR_Employee
                        where c.FirstName.Contains(term) || c.LastName.Contains(term)
                        select new { value = c.EmployeeID, label = c.FirstName + " " + c.LastName }).Distinct();
            return this.Json(name, JsonRequestBehavior.AllowGet);
        }
        public ActionResult TestAtr()
        {
            return View();
        }
    }
}