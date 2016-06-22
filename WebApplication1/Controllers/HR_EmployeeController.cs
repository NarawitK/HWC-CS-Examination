﻿using System;
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
        //private BossNameModel BNM = new BossNameModel();

        // GET: HR_Employee
        public ActionResult Index()
        {
            var hR_Employee = db.HR_Employee.Include(h => h.HR_Department);
            return View(hR_Employee.ToList());
        }

        //GET: HR_Employee/Search
        public ActionResult Search()
         {
            ViewBag.DepartmentID = new SelectList(db.HR_Department, "DepartmentID", "Name");
            return View();
         }

        [HttpGet]
        public ActionResult Search(EmployeeSearchModel searchModel) // Multiple Search Ctrl
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
            TempData["DepartmentID"] = DepartmentID;
           try
            {
                var query = (db.HR_Employee.Select(e => e.EmployeeID)).DefaultIfEmpty().Max();
                var query_substr = query.Split('-');
                var query_int = ((int.Parse(query_substr[4])) + 1).ToString("D4");
                TempData["currentID"] = query_int;
            }
            catch (Exception)
            {
                string D4 = "0000";
                TempData["currentID"] = D4;
            }
            IEnumerable < SelectListItem > BossList_create = new SelectList(db.HR_Employee, "EmployeeID","FullName");
            TempData["BossList_create"] = BossList_create;
            return View();
        }

        // POST: HR_Employee/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EmployeeID,FirstName,LastName,Birthdate,DepartmentID,BossID,ModifiedDate")] HR_Employee hR_Employee)
        {
            IEnumerable<SelectListItem> BossList_create = new SelectList(db.HR_Employee, "EmployeeID", "FullName");
            TempData["BossList_create"] = BossList_create;
            IEnumerable<SelectListItem> DepartmentID = new SelectList(db.HR_Department, "DepartmentID", "Name");
            TempData["DepartmentID"] = DepartmentID;
            try
            {
                var query = (db.HR_Employee.Select(e => e.EmployeeID)).DefaultIfEmpty().Max();
                var query_substr = query.Split('-');
                var query_int = ((int.Parse(query_substr[4])) + 1).ToString("D4");
                TempData["currentID"] = query_int;
            }
            catch (Exception)
            {
                string D4 = "0000";
                TempData["currentID"] = D4;
            }
            if (ModelState.IsValid)
            {
                    db.HR_Employee.Add(hR_Employee);
                    db.SaveChanges();
                    return RedirectToAction("Index");        
            }
            return View(hR_Employee);
        }

        // GET: HR_Employee/Edit/5
        public ActionResult Edit(string id)
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
            IEnumerable<SelectListItem> BossList  = new SelectList(db.HR_Employee, "EmployeeID", "Firstname", hR_Employee.EmployeeID);
            TempData["BossList"] = BossList;
            IEnumerable<SelectListItem> deptID = new SelectList(db.HR_Department, "DepartmentID", "Name",hR_Employee.DepartmentID);
            ViewData["deptID"] = deptID;
            return View(hR_Employee);
        }

        // POST: HR_Employee/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EmployeeID,FirstName,LastName,Birthdate,DepartmentID,BossID,ModifiedDate")] HR_Employee hR_Employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hR_Employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            IEnumerable<SelectListItem> BossList = new SelectList(db.HR_Employee, "EmployeeID", "Firstname", hR_Employee.EmployeeID);
            ViewData["BossList"] = BossList;
            IEnumerable<SelectListItem> deptID = new SelectList(db.HR_Department, "DepartmentID", "Name", hR_Employee.DepartmentID);
            ViewData["deptID"] = deptID;
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
