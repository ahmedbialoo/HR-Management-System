using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRSystem.Models
{
    public partial class Attendance
    {
        [Key]
        public int att_id { get; set; }

        [Required(ErrorMessage = "This Fild is Empty!")]
        [DataType(DataType.DateTime, ErrorMessage ="Enter a valid date!")]

        public DateTime Date { get; set; }
        [Required(ErrorMessage = "This Field is Empty!")]
        [DataType(DataType.Time, ErrorMessage ="Enetr a valid time!")]
        public TimeSpan? TimeAtt { get; set; }
        [Required(ErrorMessage = "This Field is Empty!")]
        [DataType(DataType.Time, ErrorMessage = "Enetr a valid time!")]
        public TimeSpan? TimeLeave { get; set; }
        [Required(ErrorMessage = "This Fild is Empty!")]
        [DataType(DataType.DateTime, ErrorMessage = "Enter a valid date!")]
        public DateTime order { get; set; }
        public virtual List<AttendenceEmployee> AttendenceEmployees { get; set; }

    }
}
