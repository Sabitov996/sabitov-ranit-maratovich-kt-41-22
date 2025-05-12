using Microsoft.EntityFrameworkCore;
using RanitSabitovKt_41_22.Database;
using RanitSabitovKt_41_22.Models;
using sabitov_ranit_maratovich_kt_41_22.Tests.Helpers;
using Xunit;
using System;
using System.Threading.Tasks;

namespace sabitov_ranit_maratovich_kt_41_22.Tests.Integration
{
    public class DisciplineCrudTests
    {
        [Fact]
        public async Task AddDiscipline_CreatesNewDiscipline()
        {
            // Arrange
            var context = InMemoryDbContextFactory.CreateContext();
            await context.Database.EnsureCreatedAsync();

            var discipline = new Discipline
            {
                DisciplineId = 1,
                c_discipline_name = "Физика",
            };

            // Act
            await context.Disciplines.AddAsync(discipline);
            await context.SaveChangesAsync();

            // Assert
            var createdDiscipline = await context.Disciplines.FindAsync(1);
            Assert.NotNull(createdDiscipline);
            Assert.Equal("Физика", createdDiscipline.c_discipline_name);
        }

        [Fact]
        public async Task UpdateDiscipline_ChangesDisciplineName()
        {
            // Arrange
            var context = InMemoryDbContextFactory.CreateContext();
            await context.Database.EnsureCreatedAsync();

            var discipline = new Discipline
            {
                DisciplineId = 1,
                c_discipline_name = "История",
            };
            await context.Disciplines.AddAsync(discipline);
            await context.SaveChangesAsync();

            // Act
            discipline.c_discipline_name = "История России";
            context.Disciplines.Update(discipline);
            await context.SaveChangesAsync();

            // Assert
            var updatedDiscipline = await context.Disciplines.FindAsync(1);
            Assert.Equal("История России", updatedDiscipline.c_discipline_name);
        }

        [Fact]
        public async Task DeleteDiscipline_RemovesDiscipline()
        {
            // Arrange
            var context = InMemoryDbContextFactory.CreateContext();
            await context.Database.EnsureCreatedAsync();

            var discipline = new Discipline
            {
                DisciplineId = 1,
                c_discipline_name = "Биология",
            };
            await context.Disciplines.AddAsync(discipline);
            await context.SaveChangesAsync();

            // Act
            context.Disciplines.Remove(discipline);
            await context.SaveChangesAsync();

            // Assert
            var deletedDiscipline = await context.Disciplines.FindAsync(1);
            Assert.Null(deletedDiscipline);
        }
    }
}
