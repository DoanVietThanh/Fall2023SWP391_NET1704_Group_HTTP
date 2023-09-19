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
            CreateMap<LicenseType, LicenseTypeModel>().ReverseMap();
            CreateMap<Address, AddressModel>().ReverseMap();
            CreateMap<Role, RoleModel>().ReverseMap();
            CreateMap<JobTitle, JobTitleModel>().ReverseMap();
        }
    }
}
