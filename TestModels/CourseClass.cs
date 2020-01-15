using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using System.Linq;

namespace SqlDbFrameworkNetCore.TestModels
{
    public class CourseClass
    {
        public CourseClass() { }

        [Key]
        public int CourseClassId { get; set; }

        [StringLength(10)]
        public string CourseClassCode { get; set; }

        [Range(0, 35)]
        public int RemainingSlots { get; set; }

        [Range(0,35)]
        public int MaxSlots { get; set; }

        [ForeignKey("course_id")]
        public int CourseId { get; set; }
    }
}
