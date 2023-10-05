using DocumentFormat.OpenXml.Drawing.Diagrams;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Payloads.Request
{
    public class ReviewMockTestRequest
    {
        public string Email { get; set; } 
        public int TheoryExamId { get; set; }
        
        public List<ExamGradeModel> ExamGrades { get; set; }


        //public List>
        

    }
}
