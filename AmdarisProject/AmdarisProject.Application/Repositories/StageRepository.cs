using AmdarisProject.models;
using AmdarisProject.repositories.abstractions;
using Domain.Exceptions;

namespace AmdarisProject.repositories
{
    public class StageRepository : GenericRepository<Stage>, IStageRepository
    {
        public override Stage Update(Stage stage)
        {
            if (stage is null)
                throw new APArgumentException(nameof(StageRepository), nameof(Update), nameof(stage));

            Stage stored = GetById(stage.Id);
            return stored;
        }
    }
}
