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
                    var res = searchModel.EmployeeID.Trim();
                    result = result.Where(x => x.EmployeeID.Contains(res));
                }
                if (!string.IsNullOrEmpty(searchModel.FullName))
                {
                       string FN = searchModel.FullName.Trim();
                       string[] delimiter = { " " };
                       var split = FN.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
                       int length = split.Length;
                       if (length == 1) //For Name or Surname only
                       {
                           string firstname = split[0];
                           result = result.Where(x => x.FirstName.Contains(firstname) || x.LastName.Contains(firstname));
                       }
                       else if (length >= 2) //Both Name and Surname
                    {
                           string firstname = split[0];
                           string lastname = split[1];
                           result = result.Where(x => x.FirstName.Contains(firstname) && x.LastName.Contains(lastname));
                       }
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