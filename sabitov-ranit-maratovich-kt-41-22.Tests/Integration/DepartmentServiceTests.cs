using Microsoft.EntityFrameworkCore;
using RanitSabitovKt_41_22.Database;
using RanitSabitovKt_41_22.Models;
using RanitSabitovKt_41_22.Interfaces;
using sabitov_ranit_maratovich_kt_41_22.Tests.Helpers;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using System.Collections.Generic;
using RanitSabitovKt_41_22.Filters;

namespace sabitov_ranit_maratovich_kt_41_22.Tests.Integration
{
    public class DepartmentServiceTests
    {
        [Fact]
        public async Task GetDepartmentsAsync_FoundedAfterDate_ReturnsCorrectDepartments()
        {
            // Arrange
            var context = InMemoryDbContextFactory.CreateContext();
            await context.Database.EnsureCreatedAsync();

            // Создаем преподавателей
            var teacher1 = new Teacher
            {
                TeacherId = 1,
                c_teacher_firstname = "Иван",
                c_teacher_lastname = "Иванов",
                c_teacher_middlename = "Иванович",
            };

            var teacher2 = new Teacher
            {
                TeacherId = 2,
                c_teacher_firstname = "Петр",
                c_teacher_lastname = "Петров",
                c_teacher_middlename = "Петрович",
            };

            await context.Teachers.AddRangeAsync(teacher1, teacher2);
            await context.SaveChangesAsync();

            // Создаем кафедры
            var department1 = new Department
            {
                DepartmentId = 1,
                Name = "Кафедра Информатики",
                FoundedDate = new DateTime(2010, 1, 1),
                HeadTeacherId = teacher1.TeacherId,
                Teachers = new List<Teacher> { teacher1 }
            };

            var department2 = new Department
            {
                DepartmentId = 2,
                Name = "Кафедра Математики",
                FoundedDate = new DateTime(2020, 1, 1),
                HeadTeacherId = teacher2.TeacherId,
                Teachers = new List<Teacher> { teacher2 }
            };

            await context.Departments.AddRangeAsync(department1, department2);
            await context.SaveChangesAsync();

            // Act
            var service = new DepartmentService(context);
            var filter = new DepartmentFilter
            {
                FoundedAfter = new DateTime(2015, 1, 1),
                MinTeachersCount = 1
            };

            var result = await service.GetDepartmentsAsync(filter, CancellationToken.None);

            // Assert
            Assert.Single(result); // Должна быть только одна кафедра
            var returnedDepartment = result.First();
            Assert.Equal("Кафедра Математики", returnedDepartment.Name);
            Assert.Equal(teacher2.TeacherId, returnedDepartment.HeadTeacherId);
        }
    }
}
