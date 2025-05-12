using Microsoft.EntityFrameworkCore;
using RanitSabitovKt_41_22.Database;
using RanitSabitovKt_41_22.Models;
using sabitov_ranit_maratovich_kt_41_22.Tests.Helpers;
using Xunit;
using System;
using System.Threading.Tasks;

namespace sabitov_ranit_maratovich_kt_41_22.Tests.Integration
{
    public class WorkloadCrudTests
    {
        [Fact]
        public async Task AddWorkload_CreatesNewWorkload()
        {
            // Arrange
            var context = InMemoryDbContextFactory.CreateContext();
            await context.Database.EnsureCreatedAsync();

            var teacher = new Teacher
            {
                TeacherId = 1,
                c_teacher_firstname = "Иван",
                c_teacher_lastname = "Иванов",
                c_teacher_middlename = "Иванович",
            };

            var discipline = new Discipline
            {
                DisciplineId = 1,
                c_discipline_name = "Математика",
            };

            await context.Teachers.AddAsync(teacher);
            await context.Disciplines.AddAsync(discipline);
            await context.SaveChangesAsync();

            var workload = new Workload
            {
                WorkloadId = 1,
                TeacherId = teacher.TeacherId,
                DisciplineId = discipline.DisciplineId,
                c_workload_hours = 30
            };

            // Act
            await context.Workloads.AddAsync(workload);
            await context.SaveChangesAsync();

            // Assert
            var createdWorkload = await context.Workloads
                .Include(w => w.Teacher)
                .Include(w => w.Discipline)
                .FirstOrDefaultAsync(w => w.WorkloadId == 1);

            Assert.NotNull(createdWorkload);
            Assert.Equal(30, createdWorkload.c_workload_hours);
            Assert.Equal("Иван", createdWorkload.Teacher.c_teacher_firstname);
            Assert.Equal("Математика", createdWorkload.Discipline.c_discipline_name);
        }

        [Fact]
        public async Task UpdateWorkload_ChangesWorkloadHours()
        {
            // Arrange
            var context = InMemoryDbContextFactory.CreateContext();
            await context.Database.EnsureCreatedAsync();

            var teacher = new Teacher
            {
                TeacherId = 1,
                c_teacher_firstname = "Петр",
                c_teacher_lastname = "Петров",
                c_teacher_middlename = "Петрович",
            };

            var discipline = new Discipline
            {
                DisciplineId = 1,
                c_discipline_name = "Физика",
            };

            await context.Teachers.AddAsync(teacher);
            await context.Disciplines.AddAsync(discipline);
            await context.SaveChangesAsync();

            var workload = new Workload
            {
                WorkloadId = 1,
                TeacherId = teacher.TeacherId,
                DisciplineId = discipline.DisciplineId,
                c_workload_hours = 25
            };

            await context.Workloads.AddAsync(workload);
            await context.SaveChangesAsync();

            // Act
            workload.c_workload_hours = 40;
            context.Workloads.Update(workload);
            await context.SaveChangesAsync();

            // Assert
            var updatedWorkload = await context.Workloads.FindAsync(1);
            Assert.Equal(40, updatedWorkload.c_workload_hours);
        }
    }
}
