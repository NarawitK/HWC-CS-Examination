using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HR_EmployeeController : Controller
    {
        private HREntities db = new HREntities();

        // GET: HR_Employee
        public ActionResult Index()
        {
            var hR_Employee = db.HR_Employee.Include(h => h.HR_Department);
            return View(hR_Employee.ToList());
        }

        //GET: HR_Employee/ListEmployee <Search Employee>
        public ActionResult ListEmployee()
         {
            ViewBag.DepartmentID = new SelectList(db.HR_Department, "DepartmentID", "Name");
            return View();
         }

        [HttpGet]
        public ActionResult ListEmployee(EmployeeSearchModel searchModel) // Multiple Search
        {
            ViewBag.DepartmentID = new SelectList(db.HR_Department, "DepartmentID", "Name");
            var search = new EmployeeSearchLogic();
            var model = search.GetSearchResult(searchModel);
            return View(model);
        }

        // GET: HR_Employee/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HR_Employee hR_Employee = db.HR_Employee.Find(id);
            if (hR_Employee == null)
            {
                return HttpNotFound();
            }
            return View(hR_Employee);
        }

        // GET: HR_Employee/Create
        public ActionResult Create()
        {
            IEnumerable<SelectListItem> DepartmentID = new SelectList(db.HR_Department, "DepartmentID", "Name");
            ViewBag.DepartmentID = DepartmentID;
            try
            {
                var query = (db.HR_Employee.Select(e => e.EmployeeID)).DefaultIfEmpty().Max();
                var query_substr = query.Split('-');
                var query_int = ((int.Parse(query_substr[3])) + 1).ToString("D4");
                ViewData["currentID"] = query_int;
            }
            catch
            {
                string D4 = "0001";
                ViewData["currentID"] = D4;
            }
            return View();
        }

        // POST: HR_Employee/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EmployeeID,FirstName,LastName,Birthdate,DepartmentID,BossID,ModifiedDate")] HR_Employee hR_Employee)
        {
            IEnumerable<SelectListItem> DepartmentID = new SelectList(db.HR_Department, "DepartmentID", "Name");
            ViewBag.DepartmentID = DepartmentID;
            try
            {
                var query = (db.HR_Employee.Select(e => e.EmployeeID)).Max();
                var query_substr = query.Split('-');
                if(int.Parse(query_substr[3]) == 9999)
                {
                    return View();
                }
                else
                {
                    var query_int = ((int.Parse(query_substr[3])) + 1).ToString("D4");
                    ViewData["currentID"] = query_int;
                }

            }
            catch
            {
                string D4 = "0001";
                ViewData["currentID"] = D4;
            }
            if (ModelState.IsValid)
            {
                    db.HR_Employee.Add(hR_Employee);
                    db.SaveChanges();
                    return RedirectToAction("Index");
            }
            return View(hR_Employee);
        }

        //For BossName Search
        [HttpGet]
        public ActionResult GetBossName(string term)
        {
            var name = (from c in db.HR_Employee
                        where c.EmployeeID == term
                        select new { label = c.FirstName + " " + c.LastName, value = c.EmployeeID, }).Distinct();
            if (!name.Any())
            {
                return null;
            }
            else
            {
                return Json(name.ToList(), JsonRequestBehavior.AllowGet);
            }
        }

        // GET: HR_Employee/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HR_Employee hR_Employee = db.HR_Employee.Find(id);
            IEnumerable<SelectListItem> DepartmentID = new SelectList(db.HR_Department, "DepartmentID", "Name", hR_Employee.DepartmentID);
            ViewBag.DepartmentID = DepartmentID;
            if (hR_Employee == null)
            {
                return HttpNotFound();
            }

            return View(hR_Employee);
        }

        // POST: HR_Employee/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EmployeeID,FirstName,LastName,Birthdate,DepartmentID,BossID,ModifiedDate")] HR_Employee hR_Employee)
        {
            IEnumerable<SelectListItem> DepartmentID = new SelectList(db.HR_Department, "DepartmentID", "Name", hR_Employee.DepartmentID);
            ViewBag.DepartmentID = DepartmentID;
            if (ModelState.IsValid)
            {
                db.Entry(hR_Employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");

            }
            return View(hR_Employee);
        }

        // GET: HR_Employee/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HR_Employee hR_Employee = db.HR_Employee.Find(id);
            if (hR_Employee == null)
            {
                return HttpNotFound();
            }
            return View(hR_Employee);
        }

        // POST: HR_Employee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            HR_Employee hR_Employee = db.HR_Employee.Find(id);
            db.HR_Employee.Remove(hR_Employee);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
