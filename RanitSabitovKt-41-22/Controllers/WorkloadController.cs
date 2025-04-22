using Microsoft.AspNetCore.Mvc;
using RanitSabitovKt_41_22.Database;
using RanitSabitovKt_41_22.Filters;
using RanitSabitovKt_41_22.Interfaces;
using RanitSabitovKt_41_22.Models;

namespace RanitSabitovKt_41_22.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WorkloadController : ControllerBase
    {
        private readonly IWorkloadService _workloadService;
        private readonly UniversityDbContext _dbContext;

        public WorkloadController(IWorkloadService workloadService, UniversityDbContext dbContext)
        {
            _workloadService = workloadService;
            _dbContext = dbContext;
        }

        // Получение с фильтрацией
        [HttpPost("filter")]
        public async Task<IActionResult> GetWorkloadsAsync(WorkloadFilter filter, CancellationToken cancellationToken)
        {
            var workloads = await _workloadService.GetWorkloadsAsync(filter, cancellationToken);
            return Ok(workloads);
        }

        // Добавление нагрузки
        [HttpPost("add")]
        public async Task<IActionResult> AddWorkload([FromBody] WorkloadDto dto)
        {
            var workload = new Workload
            {
                TeacherId = dto.TeacherId,
                DisciplineId = dto.DisciplineId,
                c_workload_hours = dto.C_Workload_Hours
            };

            _dbContext.Workloads.Add(workload);
            await _dbContext.SaveChangesAsync();

            return Ok(workload);
        }

        // Изменение нагрузки
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateWorkload(int id, [FromBody] WorkloadDto dto)
        {
            var workload = await _dbContext.Workloads.FindAsync(id);
            if (workload == null)
                return NotFound();

            workload.TeacherId = dto.TeacherId;
            workload.DisciplineId = dto.DisciplineId;
            workload.c_workload_hours = dto.C_Workload_Hours;

            await _dbContext.SaveChangesAsync();
            return Ok(workload);
        }
    }
}