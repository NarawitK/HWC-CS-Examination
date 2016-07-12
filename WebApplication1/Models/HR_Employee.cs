﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApplication1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;


    public partial class HR_Employee
    {
        public string EmployeeID { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }

        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "กรุณาเลือกวันเกิดด้วย")]
        public DateTime? Birthdate { get; set; }
        public int DepartmentID { get; set; }
        public string BossID { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? ModifiedDate { get; set; }

        public string BossName
        {
            get
            {
                using (HREntities db = new HREntities())
                {
                    var BName = (from q in db.HR_Employee
                                 where BossID == q.EmployeeID
                                 select q.Firstname + " " + q.Lastname).FirstOrDefault();
                    return BName;
                }

            }
        }
        public string FullName
        {
            get
            {
                return Firstname + " " + Lastname;
            }
        }

        public virtual HR_Department HR_Department { get; set; }
    }
}
