using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Services;
using WebApplication1.Models;

namespace WebApplication1.Exam_Webservice
{
    /// <summary>
    /// Search Employee (ListEmployee) Web Service
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    //[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    //[System.Web.Script.Services.ScriptService]

    [Serializable()]
    public class ListEmployeeModel
    {
        public string EmployeeID { get; set;}
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? Birthdate { get; set; }
        public string DepartmentName { get; set; }
        public string FullName {
            get {
                return FirstName+" "+ LastName;
            }
        }
    }
    public class EmployeeService : System.Web.Services.WebService
    {
        [WebMethod(MessageName = "ShowEmployeeList")]
        public List<ListEmployeeModel> ListEmployee()
        {
            using (HREntities db = new HREntities())
            {
                var Emplist = from query in db.HR_Employee
                              select new ListEmployeeModel
                              {
                                  EmployeeID = query.EmployeeID,
                                  FirstName = query.FirstName,
                                  LastName = query.LastName,
                                  Birthdate = query.Birthdate,
                                  DepartmentName = query.HR_Department.Name
                              };
                return Emplist.ToList();
            }
        }
            [WebMethod(MessageName = "FilterEmployeeList")]
            public List<ListEmployeeModel> ListEmployee(string EmployeeID, string FullName, int? DepartmentID)
            {
            using (HREntities db = new HREntities())
            {
                var query = db.HR_Employee.AsQueryable();
                if (!string.IsNullOrEmpty(EmployeeID))
                {
                    var TrimEmpID = EmployeeID.Trim();
                    query = query.Where(x => x.EmployeeID == TrimEmpID);
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
                        query = query.Where(x => x.FirstName.Contains(firstname) || x.LastName.Contains(firstname));
                    }
                    else if (length >= 2) //Search Both Name and Surname
                    {
                        string firstname = split[0];
                        string lastname = split[1];
                        query = query.Where(x => x.FirstName.Contains(firstname) && x.LastName.Contains(lastname));
                    }
                }
                if (DepartmentID.HasValue)
                {
                    query = query.Where(x => x.DepartmentID == DepartmentID);
                }
                var ResultSet = from item in query
                          select new ListEmployeeModel
                          {
                              EmployeeID = item.EmployeeID,
                              FirstName = item.FirstName,
                              LastName = item.LastName,
                              Birthdate = item.Birthdate,
                              DepartmentName = item.HR_Department.Name
                          };
                return ResultSet.ToList();
            }
        }   
    }
}
