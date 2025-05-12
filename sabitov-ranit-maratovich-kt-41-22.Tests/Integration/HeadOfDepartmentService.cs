using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RanitSabitovKt_41_22.Database;
using RanitSabitovKt_41_22.Filters;
using RanitSabitovKt_41_22.Models;
using sabitov_ranit_maratovich_kt_41_22.Tests.Helpers;
using Xunit;
using Xunit.Abstractions;

namespace sabitov_ranit_maratovich_kt_41_22.Tests.Integration
{
    public class HeadOfDepartmentServiceTests
    {
        private readonly ITestOutputHelper _output;

        public HeadOfDepartmentServiceTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task GetHeadTeachersByDisciplineNameAsync_WithMultipleDepartments_ReturnsEachHead()
        {
            // Arrange
            var context = InMemoryDbContextFactory.CreateContext(); 
            await context.Database.EnsureCreatedAsync();
            await TestDataSeeder.SeedAsync(context); 

            var service = new HeadOfDepartmentService(context);

            // Act
            var result = await service.GetHeadTeachersByDisciplineNameAsync("Матем");

            // Assert
            Assert.Equal(1, result.Count); 

            _output.WriteLine($"Найдено заведующих: {result.Count}");
            foreach (var head in result)
            {
                _output.WriteLine($"ФИО: {head.HeadTeacherFullName}, Кафедра: {head.DepartmentName}");
            }
        }

    }
}
