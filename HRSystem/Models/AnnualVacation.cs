using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRSystem.Models
{
    public class AnnualVacation
    {

        [Key]
        public int id { get; set; }

        [Required(ErrorMessage ="This Field Is Required")]
        [DataType(DataType.DateTime, ErrorMessage ="Enter a valid data!")]   
        public DateTime date { get; set; }


        [RegularExpression("^[A-Z][a-zA-Z]*$", ErrorMessage ="Enter a valid name!")]
        [Required(ErrorMessage ="This Field Is Required")]
        public string  name { get; set; }

    }
}
