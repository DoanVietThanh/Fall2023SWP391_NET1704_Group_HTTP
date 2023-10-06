using AutoMapper;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Mapping
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper()
        {
            CreateMap<Account, AccountModel>().ReverseMap();
            CreateMap<Member, MemberModel>().ReverseMap();
            CreateMap<Staff, StaffModel>().ReverseMap();
            CreateMap<Course, CourseModel>().ReverseMap();
            CreateMap<Curriculum, CurriculumModel>().ReverseMap();
            CreateMap<LicenseType, LicenseTypeModel>().ReverseMap();
            CreateMap<Address, AddressModel>().ReverseMap();
            CreateMap<Role, RoleModel>().ReverseMap();
            CreateMap<JobTitle, JobTitleModel>().ReverseMap();
            CreateMap<FeedBack, FeedBackModel>().ReverseMap();
            CreateMap<PaymentType, PaymentTypeModel>().ReverseMap();
            CreateMap<LicenseRegisterForm, LicenseRegisterFormModel>().ReverseMap();
            CreateMap<LicenseRegisterFormStatus, LicenseRegisterFormStatusModel>().ReverseMap();
            CreateMap<WeekdaySchedule, WeekdayScheduleModel>().ReverseMap();
            CreateMap<CourseReservation, CourseReservationModel>().ReverseMap();
            CreateMap<Question, QuestionModel>().ReverseMap();
            CreateMap<QuestionAnswer,AnswerModel>().ReverseMap();
            CreateMap<Vehicle, VehicleModel>().ReverseMap();
            CreateMap<VehicleType, VehicleTypeModel>().ReverseMap();
            CreateMap<Slot, SlotModel>().ReverseMap();
            CreateMap<TeachingSchedule, TeachingScheduleModel>().ReverseMap();
            CreateMap<RollCallBook, RollCallBookModel>().ReverseMap();
            CreateMap<TheoryExam,TheoryExamModel>().ReverseMap();
            CreateMap<ExamGrade, ExamGradeModel>().ReverseMap();
            CreateMap<ExamHistory, ExamHistoryModel>().ReverseMap();
        }
    }
}