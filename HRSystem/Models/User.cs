
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

 namespace HRSystem.Models

{

    public partial class User
    {
        [Key]
        [Required(ErrorMessage = "*")]
        public int User_Id { get; set; }
        [Required(ErrorMessage = "This Field is required!")]
        [RegularExpression("^[A-Z][a-zA-Z]*$", ErrorMessage = "Name Must start with capital letter and contains only letters")]
        [ MinLength(3,ErrorMessage ="minimum length is 3 letters")]
        
        public string User_Fname { get; set; }
        [Required(ErrorMessage = "This Field is required")]
        [MinLength(3,ErrorMessage = "Invalid Username")]
        public string Username { get; set; }
        [Required(ErrorMessage = "This Field is required")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage ="Minimum length is 6 characters!")]
        public string Password { get; set; }

        [Required(ErrorMessage = "This Field Is Required")]
        [EmailAddress(ErrorMessage = "Please provide a valid email address... me@example.com")]
        public string User_Email { get; set; }
        public int GroupId { get; set; }

        public virtual Group? Group { get; set; } 
    }
}

