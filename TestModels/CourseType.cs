using System.ComponentModel.DataAnnotations;

namespace FinalProjectEMS.Models
{
    public class CourseType
    {
        public CourseType() { }

        public int CourseTypeId { get; set; }

        [StringLength(30)]
        [DataType(DataType.Text)]
        public string CourseTypeName { get; set; }

        public int PricePerCredit { get; set; }
    }
}
