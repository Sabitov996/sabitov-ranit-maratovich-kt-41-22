using RanitSabitovKt_41_22.Database;
using RanitSabitovKt_41_22.Filters;
using Microsoft.EntityFrameworkCore;
using RanitSabitovKt_41_22.Models;

namespace RanitSabitovKt_41_22.Interfaces
{
    public interface IDisciplineService
    {
        Task<IEnumerable<Discipline>> GetDisciplinesAsync(DisciplineFilter filter, CancellationToken cancellationToken);
    }

    public class DisciplineService : IDisciplineService
    {
        private readonly UniversityDbContext _dbContext;

        public DisciplineService(UniversityDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Discipline>> GetDisciplinesAsync(DisciplineFilter filter, CancellationToken cancellationToken)
        {
            var query = _dbContext.Workloads
                .Include(w => w.Discipline)
                .AsQueryable();

            if (filter.TeacherId.HasValue)
            {
                query = query.Where(w => w.TeacherId == filter.TeacherId.Value);
            }

            if (filter.WorkloadMin.HasValue)
            {
                query = query.Where(w => w.c_workload_hours >= filter.WorkloadMin.Value);
            }

            if (filter.WorkloadMax.HasValue)
            {
                query = query.Where(w => w.c_workload_hours <= filter.WorkloadMax.Value);
            }

            // Возвращаем только дисциплины (уникально)
            return await query
                .Select(w => w.Discipline)
                .Distinct()
                .ToListAsync(cancellationToken);
        }
    }
}
