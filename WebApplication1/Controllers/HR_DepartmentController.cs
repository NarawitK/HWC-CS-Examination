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
    public class HR_DepartmentController : Controller
    {
        private HREntities db = new HREntities();

        // GET: HR_Department
        public ActionResult Index()
        {
            return View(db.HR_Department.ToList());
        }

        // GET: HR_Department/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HR_Department hR_Department = db.HR_Department.Find(id);
            if (hR_Department == null)
            {
                return HttpNotFound();
            }
            return View(hR_Department);
        }

        // GET: HR_Department/Create
        public ActionResult Create()
        {
            try
            {
                TempData["NewDeptID"] = db.HR_Department.Select(e => e.DepartmentID).DefaultIfEmpty().Max()+1;
            }
            catch(Exception)
            {
                TempData["NewDeptID"] = 0;
            }
                                    
            return View();
        }

        // POST: HR_Department/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="DepartmentID,Name,GroupName,ModifiedDate")] HR_Department hR_Department)
        {
            if (ModelState.IsValid)
            {
                db.HR_Department.Add(hR_Department);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(hR_Department);
        }

        // GET: HR_Department/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HR_Department hR_Department = db.HR_Department.Find(id);
            if (hR_Department == null)
            {
                return HttpNotFound();
            }
            return View(hR_Department);
        }

        // POST: HR_Department/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DepartmentID,Name,GroupName,ModifiedDate")] HR_Department hR_Department)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hR_Department).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(hR_Department);
        }

        // GET: HR_Department/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HR_Department hR_Department = db.HR_Department.Find(id);
            if (hR_Department == null)
            {
                return HttpNotFound();
            }
            return View(hR_Department);
        }

        // POST: HR_Department/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HR_Department hR_Department = db.HR_Department.Find(id);
            db.HR_Department.Remove(hR_Department);
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