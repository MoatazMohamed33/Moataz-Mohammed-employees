using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class GroupedProject
    {
        public int ProjectID { get; set; }
        public List<Employee> Emps { get; set; }
        public double OverLappedDays { get; set; }
    }
}
