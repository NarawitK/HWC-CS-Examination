using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class IDEmpLogic
    {
        private HREntities db;
        public IDEmpLogic() {
            db = new HREntities();
        }
        public IQueryable<HR_Employee> ShowResult(IDEmpModel model)
        {
            var result = db.HR_Employee.AsQueryable();
            if (model != null)
            {
                result = result.Where(x => x.BossID == x.EmployeeID);
            }
            return result;
        }
    }
}