using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RanitSabitovKt_41_22.Database;
using sabitov_ranit_maratovich_kt_41_22.Tests.Helpers;
using sabitov_ranit_maratovich_kt_41_22.Tests.Helpers.Validators;
using Xunit;

namespace sabitov_ranit_maratovich_kt_41_22.Tests.Integration
{
    public class AcademicDegreeValidTests
    {
        [Fact]
        public async Task AllSeededDegrees_ShouldHaveValidNames()
        {
            // Arrange:
            var options = new DbContextOptionsBuilder<UniversityDbContext>()
                .UseInMemoryDatabase(databaseName: "UniversityTestDb")
                .Options;

            using var context = new UniversityDbContext(options);
            await TestDataSeeder.SeedAsync(context);

            // Act:
            var degrees = await context.AcademicDegrees.ToListAsync();
            var allValid = degrees.All(degree => AcademicDegreeValidator.IsValidName(degree.c_academic_degree_name));

            // Assert:
            Assert.True(allValid);
        }
    }
}
