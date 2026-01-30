using System;
using System.Collections.Generic;
using System.Text;

namespace DtoLayer.EducationDto
{
    public class UpdateEducationDto
    {
        //public Guid Id { get; set; } // --> güncelleme işleminde ıdyi almamız gerekir
        public string EducationDegree { get; set; } = string.Empty;
        public DateTime EducationStartDate { get; set; }
        public DateTime? EducationFinishDate { get; set; }
        public string EducationSchoolName { get; set; } = string.Empty;
        public string EducationDescription { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }
    }
}
