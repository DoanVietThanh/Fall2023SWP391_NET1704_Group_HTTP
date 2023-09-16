using AutoMapper;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Services.impl;
using Microsoft.EntityFrameworkCore;

namespace DriverLicenseLearningSupport.Services
{
    public class LicenseTypeService : ILicenseTypeService
    {
        private readonly DriverLicenseLearningSupportContext _context;
        private readonly IMapper _mapper;

        public LicenseTypeService(DriverLicenseLearningSupportContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<LicenseTypeModel>> GetAllAsync()
        {
            var licenseTypesEntity = await _context.LicenseTypes.ToListAsync();
            var result = _mapper.Map<IEnumerable<LicenseTypeModel>>(licenseTypesEntity);
            return result;
        }
    }
}
