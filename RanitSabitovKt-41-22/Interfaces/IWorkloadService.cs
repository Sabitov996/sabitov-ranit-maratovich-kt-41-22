using RanitSabitovKt_41_22.Database;
using RanitSabitovKt_41_22.Filters;
using Microsoft.EntityFrameworkCore;
using RanitSabitovKt_41_22.Models;

namespace RanitSabitovKt_41_22.Interfaces
{
    public interface IWorkloadService
    {
        Task<IEnumerable<Workload>> GetWorkloadsAsync(WorkloadFilter filter, CancellationToken cancellationToken);

        Task<List<HeadTeacherDto>> GetHeadTeachersByDisciplineNameAsync(string disciplineName);
    }

    public class WorkloadService : IWorkloadService
    {
        private readonly UniversityDbContext _dbContext;

        public WorkloadService(UniversityDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Workload>> GetWorkloadsAsync(WorkloadFilter filter, CancellationToken cancellationToken)
        {
            var query = from workload in _dbContext.Workloads
                        join teacher in _dbContext.Teachers on workload.TeacherId equals teacher.TeacherId
                        join department in _dbContext.Departments on teacher.DepartmentId equals department.DepartmentId
                        join discipline in _dbContext.Disciplines on workload.DisciplineId equals discipline.DisciplineId
                        where (!filter.TeacherId.HasValue || workload.TeacherId == filter.TeacherId.Value)
                              && (!filter.DepartmentId.HasValue || teacher.DepartmentId == filter.DepartmentId.Value)
                              && (!filter.DisciplineId.HasValue || workload.DisciplineId == filter.DisciplineId.Value)
                        select new Workload
                        {
                            WorkloadId = workload.WorkloadId,
                            TeacherId = workload.TeacherId,
                            DisciplineId = workload.DisciplineId,
                            c_workload_hours = workload.c_workload_hours,
                            Teacher = teacher,
                            Discipline = discipline
                        };

            return await query.ToListAsync(cancellationToken);
        }

        public async Task<List<HeadTeacherDto>> GetHeadTeachersByDisciplineNameAsync(string disciplineName)
        {
            if (string.IsNullOrEmpty(disciplineName))
            {
                throw new ArgumentException("Имя дисциплины обязательно для заполнения.", nameof(disciplineName));
            }

            return await (from workload in _dbContext.Workloads
                          join teacher in _dbContext.Teachers on workload.TeacherId equals teacher.TeacherId
                          join department in _dbContext.Departments on teacher.DepartmentId equals department.DepartmentId
                          join headTeacher in _dbContext.Teachers on department.HeadTeacherId equals headTeacher.TeacherId
                          join discipline in _dbContext.Disciplines on workload.DisciplineId equals discipline.DisciplineId
                          where discipline.c_discipline_name.ToLower() == disciplineName.ToLower()
                          select new HeadTeacherDto
                          {
                              HeadTeacherId = headTeacher.TeacherId,
                              HeadTeacherFullName = headTeacher.c_teacher_lastname + " " + headTeacher.c_teacher_firstname + " " + headTeacher.c_teacher_middlename,
                              DepartmentName = department.Name
                          })
                          .Distinct()
                          .ToListAsync();
        }
    }
}
