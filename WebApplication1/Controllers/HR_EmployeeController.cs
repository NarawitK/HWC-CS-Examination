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


        // GET: HR_Employee with Pagination, Sort and SearchBox 
        public ActionResult Index(string sortOrder,string Search,string currentFilter,int? page,int pagesize = 15)
        {
            var hR_Employee = db.HR_Employee.Include(h => h.HR_Department);
            ViewBag.CurrentSort = sortOrder;
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

        //GET: /SearchEmployeeFromDB (Non-Webservice)
        public ActionResult SearchEmployee()
        {
            ViewBag.SearchMode = 0;
            ViewBag.DepartmentID = new SelectList(db.HR_Department, "DepartmentID", "Name");
            var model_obj = new EmployeeSearchLogic();
            var model = model_obj.ShowEmployeeList();
            return View("ListEmployee",model);
        }

        //POST: /SearchEmployeeFromDB (Non-Webservice)
        [HttpPost]
        public ActionResult SearchEmployee(EmployeeSearchModel searchModel)
        {
            ViewBag.SearchMode = 0;
            ViewBag.DepartmentID = new SelectList(db.HR_Department, "DepartmentID", "Name");
            var SearchMethod = new EmployeeSearchLogic();
            var model = (SearchMethod.GetSearchResult(searchModel)).ToList();
            return View("ListEmployee", model);
        }

        //GET: HR_Employee/ListEmployee <Search Employee> (Webservice)
        public ActionResult ListEmployee()
         {
            ViewBag.SearchMode = 1;
            ViewBag.DepartmentID = new SelectList(db.HR_Department, "DepartmentID", "Name");
            using (Exam_Webservice.EmployeeService emp_service = new Exam_Webservice.EmployeeService())
            {
                try
                {
                    var model = emp_service.ShowEmployeeList();
                    return View("ListEmployee",model);
                }
                catch(Exception e)
                {
                    ViewBag.Exception = e.GetType();
                    return View("ListEmployee");
                }
                
            };
         }

        [HttpPost] //POST: Call Webservice
        public ActionResult ListEmployee(string EmployeeID, string FullName, int? DepartmentID)
        {
            ViewBag.SearchMode = 1;
            ViewBag.DepartmentID = new SelectList(db.HR_Department, "DepartmentID", "Name");
            using (Exam_Webservice.EmployeeService emp_service = new Exam_Webservice.EmployeeService())
            {
                try
                {
                    var model = emp_service.ListEmployee(EmployeeID, FullName, DepartmentID);
                    return View("ListEmployee",model);
                }
                catch
                {
                    return RedirectToAction("Index");
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
            ViewBag.Mode = "Create";
            IEnumerable<SelectListItem> DepartmentID = new SelectList(db.HR_Department, "DepartmentID", "Name");
            IEnumerable<SelectListItem> BossID = new SelectList(db.HR_Employee, "EmployeeID", "FullName");
            ViewBag.BossIDList = BossID;
            ViewBag.DepartmentIDList = DepartmentID;
            try
            {
                var query = (db.HR_Employee.Select(e => e.EmployeeID)).DefaultIfEmpty("0001").Max();
                var query_substr = query.Split('-');
                var query_int = ((int.Parse(query_substr[3])) + 1).ToString("D4");
                ViewData["currentID"] = query_int;
            }
            catch(Exception ex)
            {
                if(ex is IndexOutOfRangeException)
                {
                    ViewData["currentID"] = "0001";
                }
                
            }
            return View("EmpEditor");
        }

        // POST: HR_Employee/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EmployeeID,FirstName,LastName,Birthdate,DepartmentID,BossID,ModifiedDate")] HR_Employee hR_Employee)
        {
            ViewBag.Mode = "Create";
            IEnumerable<SelectListItem> DepartmentID = new SelectList(db.HR_Department, "DepartmentID", "Name",hR_Employee.DepartmentID);
              IEnumerable <SelectListItem> BossID = new SelectList(db.HR_Employee, "EmployeeID", "FullName",hR_Employee.BossID);
            ViewBag.BossIDList = BossID; 
            ViewBag.DepartmentIDList = DepartmentID;
            try
            {
                var query = (db.HR_Employee.Select(e => e.EmployeeID)).DefaultIfEmpty("0001").Max();
                var query_substr = query.Split('-');
                var query_int = ((int.Parse(query_substr[3])) + 1).ToString("D4");
                ViewData["currentID"] = query_int;
            }
            catch(Exception ex) //Error show at create view
            {
                if(ex is ArgumentNullException)
                {
                    ViewBag.Exception = "เกิดข้อผิดพลาด: มีค่าว่างเกิดขึ้นระหว่างการดำเนินการ";
                    return View(hR_Employee);
                }
                if (ex is IndexOutOfRangeException)
                {
                    ViewData["currentID"] = "0001";
                }
                if(ex is DataException)
                {
                    ViewBag.Exception = ex.GetType();
                    return View(hR_Employee);
                }
                if(ex is OverflowException || ex is FormatException)
                {
                    ViewBag.Exception = "Problem Detected at Number Ordering";
                    return View(hR_Employee);
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
                        var query = (db.HR_Employee.Select(e => e.EmployeeID)).DefaultIfEmpty().Max();
                        var query_substr = query.Split('-');
                        var query_int = ((int.Parse(query_substr[3])) + 1).ToString("D4");
                        hR_Employee.EmployeeID = "EMP-" + DateTime.Today.ToString("yyyy-MM", CultureInfo.InvariantCulture) + "-" + query_int;
                        db.HR_Employee.Add(hR_Employee);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    if(ex is System.Data.Entity.Validation.DbEntityValidationException)
                    {
                        ViewBag.Exception = ex.GetType();
                        return View(hR_Employee);
                    }

                }  
            }
            return View("EmpEditor", hR_Employee);
        }

        //For BossName Search AutoComplete (Deactivated)
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
            ViewBag.Mode = "Edit";
            HR_Employee hR_Employee = db.HR_Employee.Find(id);
            IEnumerable<SelectListItem> DepartmentID = new SelectList(db.HR_Department, "DepartmentID", "Name", hR_Employee.DepartmentID);
            IEnumerable<SelectListItem> BossID = new SelectList(db.HR_Employee.Where(m => m.EmployeeID != hR_Employee.EmployeeID), "EmployeeID", "FullName", hR_Employee.BossID);
            ViewBag.DepartmentID = DepartmentID;
            ViewBag.BossIDList = BossID;
            if (hR_Employee == null)
            {
                return HttpNotFound();
            }

            return View("EmpEditor", hR_Employee);
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
                ViewBag.Mode = "Edit";
                IEnumerable<SelectListItem> DepartmentID = new SelectList(db.HR_Department, "DepartmentID", "Name", hR_Employee.DepartmentID);
                IEnumerable<SelectListItem> BossID = new SelectList(db.HR_Employee.Where(m => m.EmployeeID != hR_Employee.EmployeeID), "EmployeeID", "FullName", hR_Employee.BossID);
                ViewBag.BossIDList = BossID;
                ViewBag.DepartmentID = DepartmentID;
                if (ModelState.IsValid)
                {
                    db.Entry(hR_Employee).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }
                return View("EmpEditor",hR_Employee);
            }
            catch(Exception e)
            {
                ViewBag.Exception = e.GetType();
                return View("EmpEditor", hR_Employee);
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
