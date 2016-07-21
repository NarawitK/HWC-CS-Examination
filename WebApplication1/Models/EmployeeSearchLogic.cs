using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class EmployeeSearchLogic
    {
        public List<EmployeeSearchModel> ShowEmployeeList()
        {
            using (HREntities db = new HREntities())
            {
                var Emplist = from query in db.HR_Employee
                              select new EmployeeSearchModel
                              {
                                  EmployeeID = query.EmployeeID,
                                  FirstName = query.Firstname,
                                  LastName = query.Lastname,
                                  Birthdate = query.Birthdate,
                                  DepartmentName = query.HR_Department.Name,
                                  BossName = (from q in db.HR_Employee
                                              where query.BossID == q.EmployeeID
                                              select q.Firstname + " " + q.Lastname).FirstOrDefault()
                              };
                return Emplist.ToList();
            }
        }
        public List<EmployeeSearchModel> GetSearchResult(EmployeeSearchModel searchModel)
        {
            using (HREntities db = new HREntities())
            {
                var result = db.HR_Employee.AsQueryable();
                if (searchModel != null)
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
                            result = result.Where(x => x.Firstname.Contains(firstname) || x.Lastname.Contains(firstname));
                        }
                        else if (length >= 2) //Both Name and Surname
                        {
                            string firstname = split[0];
                            string lastname = split[1];
                            result = result.Where(x => x.Firstname.Contains(firstname) && x.Lastname.Contains(lastname)
                            || x.Firstname.Contains(lastname) && x.Lastname.Contains(firstname));
                        }
                    }
                    if (searchModel.DepartmentID.HasValue)
                    {
                        result = result.Where(x => x.DepartmentID == searchModel.DepartmentID);
                    }
                }
                var resultSet = from item in result
                                select new EmployeeSearchModel
                                {
                                    EmployeeID = item.EmployeeID,
                                    FirstName = item.Firstname,
                                    LastName = item.Lastname,
                                    Birthdate = item.Birthdate,
                                    DepartmentName = item.HR_Department.Name,
                                    BossName = (from q in db.HR_Employee
                                                where item.BossID == q.EmployeeID
                                                select q.Firstname + " " + q.Lastname).FirstOrDefault()
                                };
                return resultSet.ToList();
            }
        }
    }
}