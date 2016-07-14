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
            ViewBag.AddEmpException = TempData["AddEmpException"];
            ViewBag.EmpDelException = TempData["EmpDelException"];
            ViewBag.NameSort = string.IsNullOrEmpty(sortOrder) ? "name_desc" : null; //Sort by FirstName
            ViewBag.IDSort = string.IsNullOrEmpty(sortOrder) ? "id_desc" : null;

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
                hR_Employee = hR_Employee.Where(s => s.Firstname.Contains(Search) || s.Lastname.Contains(Search));
            }

            switch (sortOrder) //If sort link is clicked, check sort options
            {
                case "name_desc":
                    hR_Employee = hR_Employee.OrderByDescending(s=>s.Firstname);
                    break;
                case "id_desc":
                    hR_Employee = hR_Employee.OrderBy(id => id.EmployeeID);
                    break;
                default:
                    hR_Employee = hR_Employee.OrderByDescending(s => s.EmployeeID);
                    break;
            }
            int pageSize = pagesize;
            int pageNum = page ?? 1;

            return View(hR_Employee.ToPagedList(pageNum,pageSize));
        }

        //GET: /SearchEmployee (Non-Webservice)
        public ActionResult SearchEmployee()
        {
            ViewBag.DepartmentID = new SelectList(db.HR_Department, "DepartmentID", "Name");
            return View();
        }

        //GET: /SearchEmpFromDB (Non-Webservice)
        [HttpGet]
        public ActionResult SearchEmployee(EmployeeSearchModel searchModel)
        {
            ViewBag.DepartmentID = new SelectList(db.HR_Department, "DepartmentID", "Name");
            var SearchMethod = new EmployeeSearchLogic();
            var model = SearchMethod.GetSearchResult(searchModel);
            return View(model);
        }

        //GET: HR_Employee/ListEmployee <Search Employee> (Webservice)
        public ActionResult ListEmployee()
         {
            ViewBag.DepartmentID = new SelectList(db.HR_Department, "DepartmentID", "Name");
            using (Exam_Webservice.EmployeeService emp_service = new Exam_Webservice.EmployeeService())
            {
                try
                {
                    var model = emp_service.ListEmployee();
                    return View(model);
                }
                catch
                {
                    return RedirectToAction("Index");
                }
                
            };
         }

        [HttpPost] //Call Webservice
        public ActionResult ListEmployee(string EmployeeID, string FullName, int? DepartmentID)
        {
            ViewBag.DepartmentID = new SelectList(db.HR_Department, "DepartmentID", "Name");
            using (Exam_Webservice.EmployeeService emp_service = new Exam_Webservice.EmployeeService())
            {
                try
                {
                    var model = emp_service.ListEmployee(EmployeeID, FullName, DepartmentID);
                    return View(model);
                }
                catch
                {
                    return View();
                }
                
            };
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
                System.Threading.Thread.Sleep(2000);
                var query = (db.HR_Employee.Select(e => e.EmployeeID)).DefaultIfEmpty("0000").Max();
                var counter = (db.HR_Employee.Select(e => e.EmployeeID)).Count();
                var query_substr = query.Split('-');
                var query_int = ((int.Parse(query_substr[3])) + 1).ToString("D4");
                ViewData["currentID"] = query_int;
                if(counter == 10000) //If count to 1K Employees -> Send Alert 
                {
                    TempData["AddEmpException"] = "Employee At Maximum Capacity";
                    return RedirectToAction("Index");
                }
            }
            catch(Exception ex)
            {
                if(ex is IndexOutOfRangeException)
                {
                    ViewData["currentID"] = "0001";
                }
                
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
                System.Threading.Thread.Sleep(1500);
                var query = (db.HR_Employee.Select(e => e.EmployeeID)).DefaultIfEmpty("0000").Max();
                var counter = (db.HR_Employee.Select(e => e.EmployeeID)).Count();
                var query_substr = query.Split('-');
                if (counter == 10000) //When 1K Emp fire alert
                {
                    TempData["AddEmpException"] = "Employee At Maximum Capacity";
                    return RedirectToAction("Index");
                }
                else
                {
                    var query_int = ((int.Parse(query_substr[3])) + 1).ToString("D4");
                    ViewData["currentID"] = query_int;
                }
            }
            catch(Exception ex)
            {
                if(ex is ArgumentNullException)
                {
                    TempData["AddEmpException"] = "เกิดข้อผิดพลาด: มีค่าว่างเกิดขึ้นระหว่างการดำเนินการ";
                }
                if (ex is IndexOutOfRangeException)
                {
                    ViewData["currentID"] = "0001";
                }
                if(ex is DataException)
                {
                    ModelState.AddModelError(string.Empty, "Exception");
                    return View(hR_Employee);
                }
                if(ex is OverflowException || ex is FormatException)
                {
                    TempData["AddEmpException"] = "Problem Detected at Number Ordering";
                }
            }
            if (ModelState.IsValid)
            { //Handling Duplicate ID Insertion
                try
                {
                    db.HR_Employee.Add(hR_Employee);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch(Exception ex)
                {

                    if (ex is System.Data.Entity.Infrastructure.DbUpdateException)
                    {
                        System.Threading.Thread.Sleep(1300);
                        var query = (db.HR_Employee.Select(e => e.EmployeeID)).DefaultIfEmpty().Max();
                        var query_substr = query.Split('-');
                        var query_int = ((int.Parse(query_substr[3])) + 1).ToString("D4");
                        hR_Employee.EmployeeID = "EMP-" + DateTime.Today.ToString("yyyy-MM", CultureInfo.InvariantCulture) + "-" + query_int;
                        db.HR_Employee.Add(hR_Employee);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }

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
                        select new { label = c.Firstname + " " + c.Lastname, value = c.EmployeeID, }).Distinct();
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
            try
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
            catch(Exception e)
            {
                ViewBag.EmpEditException = e.GetType();
                return View(hR_Employee);
            }
        }

        // POST: HR_Employee/Delete/5
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
            try
            {
                db.HR_Employee.Remove(hR_Employee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch(Exception e)
            {
                TempData["EmpDelException"] = e.GetType();
                return RedirectToAction("Index");
            }
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
