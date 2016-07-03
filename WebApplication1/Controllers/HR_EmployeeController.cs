using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Globalization;
using System.Web.Mvc;
using WebApplication1.Models;
using PagedList;

namespace WebApplication1.Controllers
{
    public class HR_EmployeeController : Controller
    {
        private HREntities db = new HREntities();


        // GET: HR_Employee with Pagination, Sort and SearchBox //Pagination Customizable 
        public ActionResult Index(string sortOrder,string Search,string currentFilter,int? page,int pagesize = 15)
        {
            var hR_Employee = db.HR_Employee.Include(h => h.HR_Department);
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSort = string.IsNullOrEmpty(sortOrder) ? "name_desc" : ""; //Sort by FirstName

            if (Search != null) //Check if srch then page = 1 
            {
                page = 1;
            }
            else
            {
                Search = currentFilter;
            }

            if (!String.IsNullOrEmpty(Search))//Search by FirstName or LastName
            {
                hR_Employee = hR_Employee.Where(s => s.FirstName.Contains(Search) || s.LastName.Contains(Search));
            }

            switch (sortOrder) //If sort link is clicked, check sort options
            {
                case "name_desc":
                    hR_Employee = hR_Employee.OrderByDescending(s=>s.FirstName);
                    break;
                default:
                    hR_Employee = hR_Employee.OrderByDescending(s => s.EmployeeID);
                    break;
            }
            int pageSize = pagesize;
            int pageNum = page ?? 1;

            return View(hR_Employee.ToPagedList(pageNum,pageSize));
        }

        //GET: HR_Employee/ListEmployee <Search Employee>
        public ActionResult ListEmployee()
         {
            ViewBag.DepartmentID = new SelectList(db.HR_Department, "DepartmentID", "Name");
            using (Exam.Webservice.EmployeeService proxy = new Exam.Webservice.EmployeeService())
            {
                var model = proxy.ListEmployee();
                return View(model.ToList());
            };
         }

        [HttpPost] //Call Webservice
        public ActionResult ListEmployee(string EmployeeID, string FullName, int? DepartmentID)
        {
            ViewBag.DepartmentID = new SelectList(db.HR_Department, "DepartmentID", "Name");
            using (Exam.Webservice.EmployeeService proxy = new Exam.Webservice.EmployeeService())
            {
                var model = proxy.ListEmployee(EmployeeID, FullName, DepartmentID);
                return View(model.ToList());
            };
        }
        /*[HttpGet]
        public ActionResult ListEmployee(EmployeeSearchModel searchModel) // Multiple Search Not using Webservice
        {
            ViewBag.DepartmentID = new SelectList(db.HR_Department, "DepartmentID", "Name");
            var search = new EmployeeSearchLogic();
            var model = search.GetSearchResult(searchModel);
            
            return View(model);
        }*/

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
            { //Handling Duplicate ID Insertion
                try
                {
                    db.HR_Employee.Add(hR_Employee);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch
                {
                    var query = (db.HR_Employee.Select(e => e.EmployeeID)).DefaultIfEmpty().Max();
                    var query_substr = query.Split('-');
                    var query_int = ((int.Parse(query_substr[3])) + 1).ToString("D4");
                    hR_Employee.EmployeeID = "EMP-" + DateTime.Today.ToString("yyyy-MM", CultureInfo.InvariantCulture) + "-" + query_int;
                    db.HR_Employee.Add(hR_Employee);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }  
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
