using AutoMapper;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Repositories.Impl;
using Microsoft.EntityFrameworkCore;

namespace DriverLicenseLearningSupport.Repositories
{
    public class LicenseTypeRepository : ILicenseTypeRepository
    {
        private readonly DriverLicenseLearningSupportContext _context;
        private readonly IMapper _mapper;

        public LicenseTypeRepository(DriverLicenseLearningSupportContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<LicenseTypeModel>> FindAllAsync()
        {
            var licenseTypesEntity = await _context.LicenseTypes.ToListAsync();
            return _mapper.Map<IEnumerable<LicenseTypeModel>>(licenseTypesEntity);
        }

        public async Task<LicenseTypeModel> FindByIdAsync(int id)
        {
            var licenseTypesEntity = await _context.LicenseTypes.Where(x => x.LicenseTypeId == id)
                                                                .FirstOrDefaultAsync();
            return _mapper.Map<LicenseTypeModel>(licenseTypesEntity);
        }
    }
}
