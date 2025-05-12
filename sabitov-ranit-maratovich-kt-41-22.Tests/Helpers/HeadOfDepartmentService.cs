using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RanitSabitovKt_41_22.Database;
using RanitSabitovKt_41_22.Models;

namespace sabitov_ranit_maratovich_kt_41_22.Tests.Helpers
{
    public class HeadTeacherDto
    {
        public int HeadTeacherId { get; set; }
        public string HeadTeacherFullName { get; set; } = null!;
        public string DepartmentName { get; set; } = null!;
    }

    public class HeadOfDepartmentService
    {
        private readonly UniversityDbContext _dbContext;

        public HeadOfDepartmentService(UniversityDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<HeadTeacherDto>> GetHeadTeachersByDisciplineNameAsync(string disciplineName)
        {
            if (string.IsNullOrEmpty(disciplineName))
                throw new ArgumentException("Имя дисциплины обязательно", nameof(disciplineName));

            var query = from workload in _dbContext.Workloads
                        join teacher in _dbContext.Teachers on workload.TeacherId equals teacher.TeacherId
                        join department in _dbContext.Departments on teacher.DepartmentId equals department.DepartmentId
                        join headTeacher in _dbContext.Teachers on department.HeadTeacherId equals headTeacher.TeacherId
                        join discipline in _dbContext.Disciplines on workload.DisciplineId equals discipline.DisciplineId
                        where discipline.c_discipline_name.ToLower() == disciplineName.ToLower()
                        select new HeadTeacherDto
                        {
                            HeadTeacherId = headTeacher.TeacherId,
                            HeadTeacherFullName = headTeacher.c_teacher_lastname + " " +
                                                  headTeacher.c_teacher_firstname + " " +
                                                  headTeacher.c_teacher_middlename,
                            DepartmentName = department.Name
                        };

            return await query.Distinct().ToListAsync();
        }
    }
}
