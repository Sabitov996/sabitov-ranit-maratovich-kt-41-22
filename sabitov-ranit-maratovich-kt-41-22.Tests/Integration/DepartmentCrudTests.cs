using Microsoft.EntityFrameworkCore;
using RanitSabitovKt_41_22.Database;
using RanitSabitovKt_41_22.Models;
using sabitov_ranit_maratovich_kt_41_22.Tests.Helpers;
using Xunit;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace sabitov_ranit_maratovich_kt_41_22.Tests.Integration
{
    public class DepartmentCrudTests
    {
        [Fact]
        public async Task AddDepartment_CreatesNewDepartment()
        {
            // Arrange
            var context = InMemoryDbContextFactory.CreateContext();
            await context.Database.EnsureCreatedAsync();

            var department = new Department
            {
                DepartmentId = 1,
                Name = "Кафедра Биологии",
                FoundedDate = DateTime.UtcNow,
                // HeadTeacherId = null // убрать строку вообще
            };

            // Act
            await context.Departments.AddAsync(department);
            await context.SaveChangesAsync();

            // Assert
            var createdDepartment = await context.Departments.FindAsync(1);
            Assert.NotNull(createdDepartment);
            Assert.Equal("Кафедра Биологии", createdDepartment.Name);
        }

        [Fact]
        public async Task UpdateDepartment_ChangesDepartmentName()
        {
            // Arrange
            var context = InMemoryDbContextFactory.CreateContext();
            await context.Database.EnsureCreatedAsync();

            var department = new Department
            {
                DepartmentId = 1,
                Name = "Старая Кафедра",
                FoundedDate = DateTime.UtcNow
            };
            await context.Departments.AddAsync(department);
            await context.SaveChangesAsync();

            // Act
            department.Name = "Новая Кафедра";
            context.Departments.Update(department);
            await context.SaveChangesAsync();

            // Assert
            var updatedDepartment = await context.Departments.FindAsync(1);
            Assert.Equal("Новая Кафедра", updatedDepartment.Name);
        }

        [Fact]
        public async Task DeleteDepartment_AlsoDeletesRelatedTeachers()
        {
            // Arrange
            var context = InMemoryDbContextFactory.CreateContext();
            await context.Database.EnsureCreatedAsync();

            var department = new Department
            {
                DepartmentId = 1,
                Name = "Кафедра Физики",
                FoundedDate = DateTime.UtcNow
            };

            var teacher1 = new Teacher
            {
                TeacherId = 1,
                c_teacher_firstname = "Иван",
                c_teacher_lastname = "Иванов",
                c_teacher_middlename = "Иванович",
                DepartmentId = 1
            };

            var teacher2 = new Teacher
            {
                TeacherId = 2,
                c_teacher_firstname = "Петр",
                c_teacher_lastname = "Петров",
                c_teacher_middlename = "Петрович",
                DepartmentId = 1
            };

            await context.Departments.AddAsync(department);
            await context.Teachers.AddRangeAsync(teacher1, teacher2);
            await context.SaveChangesAsync();

            // Act
            context.Departments.Remove(department);
            await context.SaveChangesAsync();

            // Assert
            var deletedDepartment = await context.Departments.FindAsync(1);
            var deletedTeachers = await context.Teachers.Where(t => t.DepartmentId == 1).ToListAsync();

            Assert.Null(deletedDepartment);
            Assert.Empty(deletedTeachers);
        }
    }
}
