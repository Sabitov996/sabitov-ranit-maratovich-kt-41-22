using Microsoft.EntityFrameworkCore;
using RanitSabitovKt_41_22.Database;

namespace sabitov_ranit_maratovich_kt_41_22.Tests.Helpers
{
    public static class InMemoryDbContextFactory
    {
        public static UniversityDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<UniversityDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // уникальная база для каждого теста
                .Options;

            return new UniversityDbContext(options);
        }
    }
}
