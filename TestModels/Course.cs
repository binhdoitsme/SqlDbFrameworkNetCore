using SqlDbFrameworkNetCore.Linq;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SqlDbFrameworkNetCore.TestModels
{
    public class Course
    {
        public Course() { }

        // attributes
        [Key]
        public int CourseId { get; set; }

        [StringLength(8)]
        [DataType(DataType.Text)]
        public string CourseCode { get; set; }

        [StringLength(60)]
        [DataType(DataType.Text)]
        public string CourseName { get; set; }

        public int CreditCount { get; set; }

        public int DifficultyLevel { get; set; }

        public bool Required { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        [ForeignKey("faculty_id")]
        public int FacultyId { get; set; }

        [ForeignKey("course_type_id")]
        public int CourseTypeId { get; set; }

        [ForeignKey("semester_id")]
        public int SemesterId { get; set; }

        // non-queryable fields
        [LazyLoading]
        public CourseClass[] Classes { get; set; }
        [LazyLoading]
        public CourseType CourseType { get; set; }
        [LazyLoading]
        public bool _canRegister { get; set; } = true;

        // operations
    }
}
