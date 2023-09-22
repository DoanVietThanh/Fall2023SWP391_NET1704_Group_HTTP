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

        public async Task<IEnumerable<LicenseTypeModel>> GetAllAsync()
        {
            var licenseTypesEntities = await _context.LicenseTypes.ToListAsync();
            return _mapper.Map<IEnumerable<LicenseTypeModel>>(licenseTypesEntities);
        }

        public async Task<LicenseTypeModel> GetAsync(int id)
        {
            var licenseTypesEntity = await _context.LicenseTypes.Where(x => x.LicenseTypeId == id)
                                                                .FirstOrDefaultAsync();
            return _mapper.Map<LicenseTypeModel>(licenseTypesEntity);
        }

        public async Task<LicenseTypeModel> GetByDescAsync(string licenseTypeDesc)
        {
            var licenseTypesEntity = await _context.LicenseTypes.Where(x => x.LicenseTypeDesc.Equals(licenseTypeDesc))
                                                               .FirstOrDefaultAsync();
            return _mapper.Map<LicenseTypeModel>(licenseTypesEntity);
        }
    }
}
