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
        }
    }
}