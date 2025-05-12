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
using Xunit.Abstractions;

namespace sabitov_ranit_maratovich_kt_41_22.Tests.Integration
{
    public class WorkloadServiceTests
    {
        private readonly ITestOutputHelper _output;

        public WorkloadServiceTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task GetWorkloadsAsync_FilterByTeacherDepartmentDiscipline_ReturnsCorrectWorkloads()
        {
            // Arrange
            var context = InMemoryDbContextFactory.CreateContext();
            await context.Database.EnsureCreatedAsync();

            var department = new Department
            {
                DepartmentId = 1,
                Name = "Кафедра Информатики",
                FoundedDate = DateTime.UtcNow,
                HeadTeacherId = 1
            };

            var teacher = new Teacher
            {
                TeacherId = 1,
                c_teacher_firstname = "Петр",
                c_teacher_lastname = "Петров",
                c_teacher_middlename = "Петрович",
                DepartmentId = 1
            };

            var discipline = new Discipline
            {
                DisciplineId = 1,
                c_discipline_name = "Программирование"
            };

            var workload = new Workload
            {
                WorkloadId = 1,
                TeacherId = 1,
                DisciplineId = 1,
                c_workload_hours = 40
            };

            await context.Departments.AddAsync(department);
            await context.Teachers.AddAsync(teacher);
            await context.Disciplines.AddAsync(discipline);
            await context.Workloads.AddAsync(workload);
            await context.SaveChangesAsync();

            var workloadService = new WorkloadService(context);

            var filter = new WorkloadFilter
            {
                TeacherId = 1,
                DepartmentId = 1,
                DisciplineId = 1
            };

            // Act
            var workloads = await workloadService.GetWorkloadsAsync(filter, default);

            // Assert
            var singleWorkload = Assert.Single(workloads);

            _output.WriteLine($"Teacher: {singleWorkload.Teacher.c_teacher_lastname} {singleWorkload.Teacher.c_teacher_firstname}");
            _output.WriteLine($"Department: {singleWorkload.Teacher.Department.Name}");
            _output.WriteLine($"Discipline: {singleWorkload.Discipline.c_discipline_name}");
            _output.WriteLine($"Workload hours: {singleWorkload.c_workload_hours}");

            Assert.Equal(1, singleWorkload.TeacherId);
            Assert.Equal(1, singleWorkload.DisciplineId);

            Assert.NotNull(singleWorkload.Teacher);
            Assert.Equal("Петров", singleWorkload.Teacher.c_teacher_lastname);

            Assert.NotNull(singleWorkload.Teacher.Department);
            Assert.Equal("Кафедра Информатики", singleWorkload.Teacher.Department.Name);

            Assert.NotNull(singleWorkload.Discipline);
            Assert.Equal("Программирование", singleWorkload.Discipline.c_discipline_name);
        }
    }
}
