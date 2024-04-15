using AmdarisProject.Application.Test.ModelBuilder.ModelBuilder;
using AmdarisProject.Domain.Models.CompetitorModels;

namespace AmdarisProject.Application.Test.ModelBuilder.CompetitorBuilders
{
    internal abstract class CompetitiorBuilder<T, Y>(T competitor)
        : ModelBuilder<T, Y>(competitor) where T : Competitor where Y : CompetitiorBuilder<T, Y>
    {
    }
}
