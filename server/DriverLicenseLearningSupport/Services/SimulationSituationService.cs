using AutoMapper;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Repositories.Impl;
using DriverLicenseLearningSupport.Services.Impl;

namespace DriverLicenseLearningSupport.Services
{
    public class SimulationSituationService : ISimulationSituationService
    {
        private readonly ISimulationSituationRepo _ssrepo;
        private readonly IMapper _mapper;

        public SimulationSituationService(ISimulationSituationRepo ssrepo, IMapper mapper) 
        {
            _ssrepo = ssrepo;
            _mapper = mapper;
        }
        public async Task<IEnumerable<SimulationSituationModel>> GettAllAsync()
        {
            return await _ssrepo.GettAllAsync();
        }

        public async Task<SimulationSituationModel> GetById(int id) 
        {
            return await _ssrepo.GetById(id);
        }

        public async Task<SimulationSituationModel> CreateAsync(SimulationSituationModel simulationSituation)
        {
            var ssEntity = _mapper.Map<SimulationSituation>(simulationSituation);
            return await  _ssrepo.CreateAsync(ssEntity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _ssrepo.DeleteAsync(id);
        }

        public async Task<SimulationSituationModel> UpdateSimulaitonAsync(SimulationSituationModel model, int id)
        {
            return await _ssrepo.UpdateSimulaitonAsync(model, id);
        }
    }
}
