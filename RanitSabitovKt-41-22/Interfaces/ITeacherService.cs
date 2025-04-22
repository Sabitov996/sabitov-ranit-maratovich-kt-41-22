using RanitSabitovKt_41_22.Database;
using RanitSabitovKt_41_22.Filters;
using Microsoft.EntityFrameworkCore;
using RanitSabitovKt_41_22.Models;

namespace RanitSabitovKt_41_22.Interfaces
{
    public interface ITeacherService
    {
        Task<IEnumerable<Teacher>> GetTeachersAsync(TeacherFilter filter, CancellationToken cancellationToken);
    }

    public class TeacherService : ITeacherService
    {
        private readonly UniversityDbContext _dbContext;

        public TeacherService(UniversityDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Teacher>> GetTeachersAsync(TeacherFilter filter, CancellationToken cancellationToken)
        {
            var query = _dbContext.Teachers
                .Include(t => t.Department)
                .Include(t => t.AcademicDegree)
                .Include(t => t.Staff)
                .AsQueryable();

            if (filter.DepartmentId.HasValue)
            {
                query = query.Where(t => t.DepartmentId == filter.DepartmentId.Value);
            }

            if (filter.AcademicDegreeId.HasValue)
            {
                query = query.Where(t => t.AcademicDegreeId == filter.AcademicDegreeId.Value);
            }

            if (filter.StaffId.HasValue)
            {
                query = query.Where(t => t.StaffId == filter.StaffId.Value);
            }

            return await query.ToListAsync(cancellationToken);
        }
    }
}
