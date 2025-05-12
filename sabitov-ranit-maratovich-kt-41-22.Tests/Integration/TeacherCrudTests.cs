using Microsoft.EntityFrameworkCore;
using RanitSabitovKt_41_22.Database;
using RanitSabitovKt_41_22.Models;
using sabitov_ranit_maratovich_kt_41_22.Tests.Helpers;
using Xunit;
using System;
using System.Threading.Tasks;

namespace sabitov_ranit_maratovich_kt_41_22.Tests.Integration
{
    public class TeacherCrudTests
    {
        [Fact]
        public async Task AddTeacher_CreatesNewTeacher()
        {
            // Arrange
            var context = InMemoryDbContextFactory.CreateContext();
            await context.Database.EnsureCreatedAsync();

            var teacher = new Teacher
            {
                TeacherId = 1,
                c_teacher_firstname = "Алексей",
                c_teacher_lastname = "Смирнов",
                c_teacher_middlename = "Алексеевич",
                DepartmentId = 1, // допустим, у нас есть кафедра с Id = 1
            };

            // Act
            await context.Teachers.AddAsync(teacher);
            await context.SaveChangesAsync();

            // Assert
            var createdTeacher = await context.Teachers.FindAsync(1);
            Assert.NotNull(createdTeacher);
            Assert.Equal("Алексей", createdTeacher.c_teacher_firstname);
        }

        [Fact]
        public async Task UpdateTeacher_ChangesTeacherName()
        {
            // Arrange
            var context = InMemoryDbContextFactory.CreateContext();
            await context.Database.EnsureCreatedAsync();

            var teacher = new Teacher
            {
                TeacherId = 1,
                c_teacher_firstname = "Сергей",
                c_teacher_lastname = "Петров",
                c_teacher_middlename = "Сергеевич",
                DepartmentId = 1,
            };
            await context.Teachers.AddAsync(teacher);
            await context.SaveChangesAsync();

            // Act
            teacher.c_teacher_firstname = "Илья";
            context.Teachers.Update(teacher);
            await context.SaveChangesAsync();

            // Assert
            var updatedTeacher = await context.Teachers.FindAsync(1);
            Assert.Equal("Илья", updatedTeacher.c_teacher_firstname);
        }

        [Fact]
        public async Task DeleteTeacher_RemovesTeacher()
        {
            // Arrange
            var context = InMemoryDbContextFactory.CreateContext();
            await context.Database.EnsureCreatedAsync();

            var teacher = new Teacher
            {
                TeacherId = 1,
                c_teacher_firstname = "Олег",
                c_teacher_lastname = "Сидоров",
                c_teacher_middlename = "Олегович",
                DepartmentId = 1,
            };
            await context.Teachers.AddAsync(teacher);
            await context.SaveChangesAsync();

            // Act
            context.Teachers.Remove(teacher);
            await context.SaveChangesAsync();

            // Assert
            var deletedTeacher = await context.Teachers.FindAsync(1);
            Assert.Null(deletedTeacher);
        }
    }
}
