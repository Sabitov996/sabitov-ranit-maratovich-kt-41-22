using RanitSabitovKt_41_22.Database;
using RanitSabitovKt_41_22.Filters;
using Microsoft.EntityFrameworkCore;
using RanitSabitovKt_41_22.Models;

namespace RanitSabitovKt_41_22.Interfaces
{
    public interface IWorkloadService
    {
        Task<IEnumerable<Workload>> GetWorkloadsAsync(WorkloadFilter filter, CancellationToken cancellationToken);
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
            var query = _dbContext.Workloads
                .Include(w => w.Teacher)
                    .ThenInclude(t => t.Department)
                .Include(w => w.Discipline)
                .AsQueryable();

            if (filter.TeacherId.HasValue)
            {
                query = query.Where(w => w.TeacherId == filter.TeacherId.Value);
            }

            if (filter.DepartmentId.HasValue)
            {
                query = query.Where(w => w.Teacher.DepartmentId == filter.DepartmentId.Value);
            }

            if (filter.DisciplineId.HasValue)
            {
                query = query.Where(w => w.DisciplineId == filter.DisciplineId.Value);
            }

            return await query.ToListAsync(cancellationToken);
        }
    }
}
