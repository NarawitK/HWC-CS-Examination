﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class EmpIndexModel
    {
        public string EmployeeID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Nullable<System.DateTime> Birthdate { get; set; }
        public int DepartmentID { get; set; }
        public string BossFullName { get; set; }
        public System.DateTime ModifiedDate { get; set; }
    }
}