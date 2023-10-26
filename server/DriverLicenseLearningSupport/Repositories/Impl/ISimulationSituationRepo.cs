using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Repositories.Impl
{
    public interface ISimulationSituationRepo
    {
        Task<IEnumerable<SimulationSituationModel>> GettAllAsync();

        Task<SimulationSituationModel> GetById(int id);

        Task<SimulationSituationModel> CreateAsync(SimulationSituation simulationSituation);

        Task<bool> DeleteAsync(int id);

        Task<SimulationSituationModel> UpdateSimulaitonAsync(SimulationSituationModel model, int id);
    }
}
