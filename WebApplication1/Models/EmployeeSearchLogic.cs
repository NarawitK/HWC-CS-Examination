using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class EmployeeSearchLogic
    {
        private HREntities db;
        public EmployeeSearchLogic()
        {
            db = new HREntities();
        }
        public IQueryable<HR_Employee> GetSearchResult(EmployeeSearchModel searchModel)
        {
            var result = db.HR_Employee.AsQueryable();
                if(searchModel != null)
            {
                if (!string.IsNullOrEmpty(searchModel.EmployeeID))
                {
                    result = result.Where(x => x.EmployeeID.Contains(searchModel.EmployeeID));
                }
                if (!string.IsNullOrEmpty(searchModel.FirstName))
                {
                    result = result.Where(x => x.FirstName.Contains(searchModel.FirstName));
                }
                if (!string.IsNullOrEmpty(searchModel.LastName))
                {
                    result = result.Where(x => x.LastName.Contains(searchModel.LastName));
                }
                if (searchModel.DepartmentID.HasValue)
                {
                    result = result.Where(x => x.DepartmentID == searchModel.DepartmentID);
                }
            }
            return result;
        }
    }
}