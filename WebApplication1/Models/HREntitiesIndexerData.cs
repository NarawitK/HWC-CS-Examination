using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;

namespace WebApplication1.Models
{
    public class HREntitiesIndexerData
    {
        public IEnumerable<HR_Employee> HRE { get; set; }
        public IEnumerable<HR_Department> HRD { get; set; } 
    }
}