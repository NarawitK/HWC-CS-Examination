using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
                    try
                    {
                        string FN = searchModel.FullName.Trim();
                        var split = FN.Split(' ');
                        int length = FN.Length;
                        if (length === 1)
                        {
                            string firstname = split[0];
                            result = result.Where(x => x.FirstName.Contains(firstname) || x.LastName.Contains(firstname));
                        }
                        else if (length === 2)
                        {
                            string firstname = split[0];
                            string lastname = split[1];
                            result = result.Where(x => x.FirstName.Contains(firstname) && x.LastName.Contains(lastname));
                        }
                        else
                        {
                            result = null;
                        }
                    }
                    catch
                    {

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