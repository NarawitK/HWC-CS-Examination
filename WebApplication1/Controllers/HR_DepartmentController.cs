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
    public class HR_DepartmentController : Controller
    {
        private HREntities db = new HREntities();

        // GET: HR_Department
        public ActionResult Index()
        {
            ViewBag.DeptDelException = TempData["DeptDelException"];
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
            ViewBag.Mode = 1;
            return View("DeptEditor");
        }

        // POST: HR_Department/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="DepartmentID,Name,GroupName,ModifiedDate")] HR_Department hR_Department)
        {
            ViewBag.Mode = 1;
            try
            {
                if (ModelState.IsValid)
                {
                    db.HR_Department.Add(hR_Department);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View("DeptEditor", hR_Department);
            }
            catch(Exception e)
            {
                if(e is OverflowException)
                {
                    ViewBag.CreateDeptException = "มีการใส่ค่าที่เกินข้อจำกัด กรุณาเช็คค่าที่กรอกอีกครั้ง";
                    return View(hR_Department);
                }
               ViewBag.CreateDeptException = "มีข้อผิดพลาดเกิดขึ้น ลองเช็คค่าที่ใส่ดูอีกครั้ง";
                return View(hR_Department);
            }
            
        }

        // GET: HR_Department/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.Mode = 2;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HR_Department hR_Department = db.HR_Department.Find(id);
            if (hR_Department == null)
            {
                return HttpNotFound();
            }
            return View("DeptEditor", hR_Department);
        }

        // POST: HR_Department/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DepartmentID,Name,GroupName,ModifiedDate")] HR_Department hR_Department)
        {
            ViewBag.Mode = 2;
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(hR_Department).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View("DeptEditor", hR_Department);
            }
            catch
            {
                ViewBag.EditDeptException = "เกิดข้อผิดพลาดในการแก้ไข ลองเช็คค่าที่ใส่ดูอีกครั้ง";
                return View("DeptEditor", hR_Department);
            }
            
        }

        // POST: HR_Employee/Delete/5
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
            try
            {
                db.HR_Department.Remove(hR_Department);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch(Exception e)
            {
                TempData["DeptDelException"] = "ไม่สามารถลบหน่วยงานได้ เนื่องจากยังมีพนักงานสังกัดอยู่ โปรดย้ายพนักงานออกก่อนทำการลบหน่วยงาน. Error:  "+e.GetType();
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
