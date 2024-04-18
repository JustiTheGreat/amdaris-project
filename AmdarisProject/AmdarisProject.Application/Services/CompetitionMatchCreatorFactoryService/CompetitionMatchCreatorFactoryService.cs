using AmdarisProject.Application.Abstractions;
using AmdarisProject.Domain.Models.CompetitionModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmdarisProject.Application.Services.CompetitionMatchCreatorServices
{
    public class CompetitionMatchCreatorFactoryService : ICompetitionMatchCreatorFactoryService
    {
        private Dictionary<Type, CompetitionMatchCreator> competitionMatchCreatorServices = [];

        public CompetitionMatchCreatorFactoryService(IUnitOfWork unitOfWork)
        {
            competitionMatchCreatorServices.Add(typeof(OneVSAllCompetition), new OneVsAllCompetitionMatchCreator(unitOfWork));
            competitionMatchCreatorServices.Add(typeof(TournamentCompetition), new TournamentCompetitionMatchCreator(unitOfWork));
        }

        public CompetitionMatchCreator GetCompetitionMatchCreatorService(Type type)
            => competitionMatchCreatorServices[type];
    }
}
