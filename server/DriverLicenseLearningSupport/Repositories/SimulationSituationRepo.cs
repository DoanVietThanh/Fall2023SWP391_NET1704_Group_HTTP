using AutoMapper;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Repositories.Impl;
using Microsoft.EntityFrameworkCore;

namespace DriverLicenseLearningSupport.Repositories
{
    public class SimulationSituationRepo : ISimulationSituationRepo
    {
        private readonly DriverLicenseLearningSupportContext _context;
        private readonly IMapper _mapper;

        public SimulationSituationRepo(DriverLicenseLearningSupportContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<SimulationSituationModel> CreateAsync(SimulationSituation simulationSituation)
        {
            await _context.SimulationSituations.AddAsync(simulationSituation);
            bool isSuccess = await _context.SaveChangesAsync() > 0 ? true : false;
            if (isSuccess)
            {
                var simulationSituationEntity = await _context.SimulationSituations.OrderByDescending(x => x.SimulationId)
                    .FirstOrDefaultAsync();
                simulationSituation.SimulationId = Convert.ToInt32(simulationSituationEntity.SimulationId);
            }
            return _mapper.Map<SimulationSituationModel>(simulationSituation);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var simulation = await _context.SimulationSituations.FirstOrDefaultAsync(x => x.SimulationId == id);

            if (simulation is null) { return false; }

            _context.SimulationSituations.Remove(simulation);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        public async Task<SimulationSituationModel> GetById(int id)
        {
            var ssm = await _context.SimulationSituations.FirstOrDefaultAsync(x => x.SimulationId == id);
            if (ssm is null)
            {
                return null;
            }
            return _mapper.Map<SimulationSituationModel>(ssm);
        }

        public async Task<IEnumerable<SimulationSituationModel>> GettAllAsync()
        {
            var ssm = await _context.SimulationSituations.OrderByDescending(x => x.SimulationId).ToListAsync();

            return ssm is not null ? _mapper.Map<IEnumerable<SimulationSituationModel>>(ssm) : null;
        }

        public async Task<SimulationSituationModel> UpdateSimulaitonAsync(SimulationSituationModel model, int id)
        {
            var entity = await _context.SimulationSituations.FirstOrDefaultAsync(x => x.SimulationId == id);
            if (entity is null) { return null; }
            entity.TimeResult = model.TimeResult;
            entity.ImageResult = model.ImageResult;
            return _mapper.Map<SimulationSituationModel>(entity);
        }
    }
}
