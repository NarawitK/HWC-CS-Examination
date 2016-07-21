using System;
using System.Collections.Generic;
using System.Linq;
using WebApplication1.Models;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    [Serializable()]
    public class EmployeeSearchModel
    {
        public string EmployeeID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? Birthdate { get; set; }
        public int? DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public string BossID { get; set; }
        public string BossName { get; set; }
        public string FullName { get; set; }
    }
}