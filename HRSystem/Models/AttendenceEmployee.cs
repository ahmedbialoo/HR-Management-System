using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRSystem.Models
{
    public class AttendenceEmployee
    {
        public int Emp_Ssn { get; set; }
        public virtual Employee Employees { get; set; }
        public int Att_id { get; set; }
        public virtual Attendance Attendances { get; set; }
    }
}
