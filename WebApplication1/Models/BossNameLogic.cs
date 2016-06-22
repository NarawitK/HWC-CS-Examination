using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class BossNameLogic
    {
        private HREntities db;
        public BossNameLogic()
        {
            db = new HREntities();
        }
        public IQueryable<HR_Employee> GetBossName()
        {
            var res = db.HR_Employee.AsQueryable();
            res = res.Where(x => x.EmployeeID == x.BossID);
            return res;
        }
    }
}