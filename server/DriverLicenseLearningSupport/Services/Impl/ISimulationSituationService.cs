using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Services.Impl
{
    public interface ISimulationSituationService
    {
        Task<IEnumerable<SimulationSituationModel>> GettAllAsync();
        Task<SimulationSituationModel> GetById(int id);

        Task<SimulationSituationModel> CreateAsync(SimulationSituationModel simulationSituation);
        Task<bool> DeleteAsync(int id);

        Task<SimulationSituationModel> UpdateSimulaitonAsync(SimulationSituationModel model, int id);


    }
}
