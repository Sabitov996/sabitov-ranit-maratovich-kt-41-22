using Microsoft.EntityFrameworkCore;
using RanitSabitovKt_41_22.Database;
using RanitSabitovKt_41_22.Filters;
using RanitSabitovKt_41_22.Models;

namespace RanitSabitovKt_41_22.Interfaces
{
    public interface IDepartmentService
    {
        Task<IEnumerable<Department>> GetDepartmentsAsync(DepartmentFilter filter, CancellationToken cancellationToken);
    }

    public class DepartmentService : IDepartmentService
    {
        private readonly UniversityDbContext _dbContext;

        public DepartmentService(UniversityDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Department>> GetDepartmentsAsync(DepartmentFilter filter, CancellationToken cancellationToken)
        {
            var query = _dbContext.Departments
                .Include(d => d.HeadTeacher)
                .Include(d => d.Teachers)
                .AsQueryable();

            if (filter.FoundedAfter.HasValue)
            {
                var foundedUtc = DateTime.SpecifyKind(filter.FoundedAfter.Value, DateTimeKind.Utc);
                query = query.Where(d => d.FoundedDate > foundedUtc);
            }

            if (filter.MinTeachersCount.HasValue)
            {
                query = query.Where(d => d.Teachers.Count() >= filter.MinTeachersCount.Value);
            }

            return await query.ToListAsync(cancellationToken);
        }
    }
}
