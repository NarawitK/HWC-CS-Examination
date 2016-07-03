using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Services;
using WebApplication1.Models;

namespace WebApplication1.Exam.Webservice
{
    /// <summary>
    /// Search Employee (ListEmployee) Web Service
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class EmployeeService : System.Web.Services.WebService
    {
        private HREntities db = new HREntities();

        [WebMethod]
        public IQueryable<HR_Employee> ListEmployee()
        {
                var EmpList = db.HR_Employee;
                return EmpList;
        }

        [WebMethod]
        public IQueryable<HR_Employee> ListEmployee(string EmployeeID, string FullName, int? DepartmentID)
        {
            var result = db.HR_Employee.AsQueryable();
                if (!string.IsNullOrEmpty(EmployeeID))
                {
                    var res = EmployeeID.Trim();
                    result = result.Where(x => x.EmployeeID == res);
                }
                if (!string.IsNullOrEmpty(FullName))
                {
                    string FN = FullName.Trim();
                    string[] delimiter = { " " };
                    var split = FN.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
                    int length = split.Length;
                    if (length == 1) //Search Name or Surname only
                    {
                        string firstname = split[0];
                        result = result.Where(x => x.FirstName.Contains(firstname) || x.LastName.Contains(firstname));
                    }
                    else if (length >= 2) //Search Both Name and Surname
                    {
                        string firstname = split[0];
                        string lastname = split[1];
                        result = result.Where(x => x.FirstName.Contains(firstname) && x.LastName.Contains(lastname));
                    }
                }
                if (DepartmentID.HasValue)
                {
                    result = result.Where(x => x.DepartmentID == DepartmentID);
                }
            return result;
        }
    }
}
