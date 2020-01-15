using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SqlDbFrameworkNetCore.TestModels
{
    public class Registration
    {
        public Registration() { }

        // custom constructor
        public Registration(int studentId, int courseId, int courseClassId, int semesterId)
        {
            RegistrationId = 0;
            StudentId = studentId;
            CourseId = courseId;
            CourseClassId = courseClassId;
            SemesterId = semesterId;
            Status = true;
        }

        [Key]
        public int RegistrationId { get; set; }

        public bool Status { get; set; }

        [ForeignKey("student_id")]
        public int StudentId { get; set; }

        [ForeignKey("course_id")]
        public int CourseId { get; set; }

        [ForeignKey("course_class_id")]
        public int CourseClassId { get; set; }

        [ForeignKey("semester_id")]
        public int SemesterId { get; set; }
    }
}

