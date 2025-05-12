using Microsoft.EntityFrameworkCore;
using RanitSabitovKt_41_22.Database;
using RanitSabitovKt_41_22.Filters;
using RanitSabitovKt_41_22.Interfaces;
using RanitSabitovKt_41_22.Models;
using sabitov_ranit_maratovich_kt_41_22.Tests.Helpers;
using Xunit;

namespace sabitov_ranit_maratovich_kt_41_22.Tests.Integration
{
    public class TeacherServiceTests
    {
        [Fact]
        public async Task GetTeachersAsync_FilterByDepartmentNameDegreeStaff_ReturnsCorrectTeachers()
        {
            // Arrange
            var context = InMemoryDbContextFactory.CreateContext();
            await context.Database.EnsureCreatedAsync();

            await TestDataSeeder.SeedAsync(context);

            var service = new TeacherService(context);

            string departmentName = "Кафедра физики";
            var department = await context.Departments.FirstOrDefaultAsync(d => d.Name == departmentName);
            Assert.NotNull(department); 

            var filter = new TeacherFilter
            {
                DepartmentId = department.DepartmentId
            };


            // Act
            var result = await service.GetTeachersAsync(filter, CancellationToken.None);
            var teacherList = result.ToList();



            // Assert
            Assert.Equal(2, teacherList.Count);

        }
    }
}
