using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRSystem.Models

{
    public partial class Group
    {
        [Key]
        public int Group_Id { get; set; }
       
        [MaxLength(10, ErrorMessage ="Invalid Name"), MinLength(2, ErrorMessage = "Invalid Name")]
        public string Group_Name { get; set; }
        
        public bool Emp_show { get; set; }
        public bool Emp_Add { get; set; }
        public bool Emp_Edit { get; set; }
        public bool Emp_Delete { get; set; }
        public bool User_show { get; set; }
        public bool User_Add { get; set; }
        public bool User_Edit { get; set; }
        public bool User_Delete { get; set; }
        public bool Group_show { get; set; }
        public bool Group_Add { get; set; }
        public bool Group_Edit { get; set; }
        public bool Group_Delete { get; set; }
        public bool Attend_show { get; set; }
        public bool Attend_Add { get; set; }
        public bool Attend_Edit { get; set; }
        public bool Attend_Delete { get; set; }

        public virtual List<User> Users { get; set; }
    }
}
