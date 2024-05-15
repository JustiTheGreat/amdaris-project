using AmdarisProject.Infrastructure.Persistance.Contexts;
using AmdarisProject.Presentation.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Options;

namespace AmdarisProject.Presentation.Test
{
    public class AmdarisProjectDBContextBuilder : IDisposable
    {
        private readonly AmdarisProjectDBContext _dbContext;

        public AmdarisProjectDBContextBuilder(string dbName = "TestAmdarisProject")
        {
            DbContextOptions options = new DbContextOptionsBuilder<AmdarisProjectDBContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;
            _dbContext = new AmdarisProjectDBContext(options, GetConnectionStringsOptions());
        }

        private static IOptions<ConnectionStrings> GetConnectionStringsOptions()
        {
            ConnectionStrings connectionStrings = new() { DatabaseConnection = string.Empty };
            IOptions<ConnectionStrings> options = Microsoft.Extensions.Options.Options.Create(connectionStrings);
            return options;
        }

        public AmdarisProjectDBContext GetContext()
        {
            _dbContext.Database.EnsureCreated();
            return _dbContext;
        }

        public void Dispose() { }
    }
}
