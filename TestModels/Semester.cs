using System;
using System.ComponentModel.DataAnnotations;

namespace FinalProjectEMS.Models
{
    public class Semester
    {
        public Semester() { }

        // attributes
        [Key]
        public int SemesterId { get; set; }

        [StringLength(8)]
        [DataType(DataType.Text)]
        public string SemesterName { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
    }
}
