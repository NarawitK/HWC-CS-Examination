using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class EmpIndexLogic
    {
        private HREntities db;
        public EmpIndexLogic(){
            db = new HREntities();
        }
        public IQueryable<HR_Employee> GetEmpIndex()
        {
            var result = db.HR_Employee.AsQueryable();
            result = result.Where(x => x.EmployeeID == x.BossID);
            return result;
        }
    }
}