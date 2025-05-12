using Microsoft.EntityFrameworkCore;
using RanitSabitovKt_41_22.Database;
using RanitSabitovKt_41_22.Models;
using RanitSabitovKt_41_22.Filters;
using sabitov_ranit_maratovich_kt_41_22.Tests.Helpers;
using Xunit;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System;
using RanitSabitovKt_41_22.Interfaces;

namespace sabitov_ranit_maratovich_kt_41_22.Tests.Integration
{
    public class DisciplineServiceTests
    {
        [Fact]
        public async Task GetDisciplinesAsync_FilterByTeacherAndWorkloadRange_ReturnsCorrectDisciplines()
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
                DepartmentId = 1
            };

            var department = new Department
            {
                DepartmentId = 1,
                Name = "Кафедра Математики",
                HeadTeacherId = 1,
                FoundedDate = DateTime.UtcNow
            };

            var discipline1 = new Discipline
            {
                DisciplineId = 1,
                c_discipline_name = "Математика"
            };

            var discipline2 = new Discipline
            {
                DisciplineId = 2,
                c_discipline_name = "Физика"
            };

            var workload1 = new Workload
            {
                WorkloadId = 1,
                TeacherId = 1,
                DisciplineId = 1,
                c_workload_hours = 25
            };

            var workload2 = new Workload
            {
                WorkloadId = 2,
                TeacherId = 1,
                DisciplineId = 2,
                c_workload_hours = 27
            };

            await context.Departments.AddAsync(department);
            await context.Teachers.AddAsync(teacher);
            await context.Disciplines.AddRangeAsync(discipline1, discipline2);
            await context.Workloads.AddRangeAsync(workload1, workload2);
            await context.SaveChangesAsync();

            var disciplineService = new DisciplineService(context);

            var filter = new DisciplineFilter
            {
                TeacherId = 1,
                MinWorkloadHours = 20,
                MaxWorkloadHours = 30
            };

            // Act
            var disciplines = await disciplineService.GetDisciplinesAsync(filter, default);

            // Assert
            Assert.Equal(2, disciplines.Count());

            var disciplineNames = disciplines.Select(d => d.c_discipline_name).ToList();
            Assert.Contains("Математика", disciplineNames);
            Assert.Contains("Физика", disciplineNames);
        }
    }
}
