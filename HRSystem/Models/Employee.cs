using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace HRSystem.Models
{


    public partial class Employee
    {
   

        [Key]
        public int Emp_Ssn { get; set; }



        [Required(ErrorMessage = "This Field Is Required")]
        [MinLength(3, ErrorMessage = "Must be more than 2 characters")]
        [RegularExpression(@"^[a-zA-Z]{3,}(?: [a-zA-Z]+){0,2}$", ErrorMessage = "Please provide a valid fullname")]
        public string Emp_FullName { get; set; }



        [Required(ErrorMessage = "This Field Is Required")]
        [RegularExpression("^[A-Z][a-zA-Z]*$", ErrorMessage ="Enter a valid address that starts with a capital letter")]
        public string Emp_Address { get; set; }


        [Required(ErrorMessage = "This Field Is Required")]
        [EmailAddress(ErrorMessage = "Please provide a valid email address... me@example.com")]
        public string Emp_Email { get; set; }


        [RegularExpression(@"^01[0-2,5]{1}[0-9]{8}$", ErrorMessage = "Please provide a valid Egyptian mobile number")]

        [Required(ErrorMessage = "This Field Is Required")]
        public string Phone { get; set; }



        [Required(ErrorMessage = "This Field Is Required")]
        [RegularExpression(@"^([M|m]ale|[F|f]emale)$", ErrorMessage ="Invalid!")]
        public string Emp_Gender { get; set; }



        [Required(ErrorMessage = "This Field Is Required")]
       

        public DateTime BirthDate { get; set; }



        [Required(ErrorMessage = "This Field Is Required")]
        

        public DateTime HireDate { get; set; }
        [Required(ErrorMessage = "This Field Is Required")]
        [DataType(DataType.Currency, ErrorMessage ="Invalid salary")]
        [RegularExpression("^[1-9][0-9]*$", ErrorMessage ="Invalid Salary")]
        public int Salary { get; set; }

        [RegularExpression("^[A-Z][a-zA-Z]*$", ErrorMessage = "Enter a valid nationality!")]

        [Required(ErrorMessage = "This Field Is Required")]
        public string Nationality { get; set; }



        [Required(ErrorMessage = "This Field Is Required")]
        [MaxLength(14, ErrorMessage = "National ID must be  14 digits"), MinLength(14, ErrorMessage = "National ID must be 14 digits")]
        public string Emp_NationalID { get; set; }



        [Required(ErrorMessage = "This Field Is Required")]
        public string DeptName { get; set; }



        [Required(ErrorMessage = "This Field Is Required")]
        public TimeSpan Start_Time { get; set; }



        [Required(ErrorMessage = "This Field Is Required")]
        public TimeSpan End_Time { get; set; }


        [Required(ErrorMessage = "This Field Is Required")]
        public string Vac1 { get; set; }


        [Required(ErrorMessage = "This Field Is Required")]
        public string Vac2 { get; set; }

        [Required(ErrorMessage = "This Field Is Required")]
        [RegularExpression(@"\b([1-9]|[1-9][0-9]|100)\b", ErrorMessage ="Enter a valid value")]
        public int overtime { get; set; }

        [Required(ErrorMessage = "This Field Is Required")]
        [RegularExpression(@"\b([1-9]|[1-9][0-9]|100)\b", ErrorMessage = "Enter a valid value")]
        public int discount { get; set; }


        public virtual List<AttendenceEmployee>? AttendenceEmployees { get; set; }

    }
}
