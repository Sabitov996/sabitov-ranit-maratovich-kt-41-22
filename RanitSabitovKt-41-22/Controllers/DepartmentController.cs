using Microsoft.AspNetCore.Mvc;
using RanitSabitovKt_41_22.Database;
using Microsoft.EntityFrameworkCore;
using RanitSabitovKt_41_22.Filters;
using RanitSabitovKt_41_22.Interfaces;
using RanitSabitovKt_41_22.Models;

namespace RanitSabitovKt_41_22.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DepartmentController : ControllerBase
    {
        private readonly UniversityDbContext _dbContext;

        public DepartmentController(UniversityDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpPost("add")]
        public async Task<IActionResult> AddDepartment([FromBody] DepartmentDto dto)
        {
            var department = new Department
            {
                Name = dto.Name,
                FoundedDate = DateTime.SpecifyKind(dto.FoundedDate, DateTimeKind.Utc), // 👈 добавлено
                HeadTeacherId = dto.HeadTeacherId
            };

            _dbContext.Departments.Add(department);
            await _dbContext.SaveChangesAsync();
            return Ok(department);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateDepartment(int id, [FromBody] DepartmentUpdateDto dto)
        {
            var department = await _dbContext.Departments.FindAsync(id);
            if (department == null)
                return NotFound();

            department.Name = dto.Name;
            department.FoundedDate = DateTime.SpecifyKind(dto.FoundedDate, DateTimeKind.Utc);
            department.HeadTeacherId = dto.HeadTeacherId;

            await _dbContext.SaveChangesAsync();
            return Ok(department);
        }
        //Удаление кафедры (вместе с преподавателями)
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var department = await _dbContext.Departments
                .Include(d => d.Teachers)
                .FirstOrDefaultAsync(d => d.DepartmentId == id);

            if (department == null)
                return NotFound();

            _dbContext.Departments.Remove(department);
            await _dbContext.SaveChangesAsync();

            return Ok($"Кафедра (ID = {id}) и все связанные преподаватели удалены");
        }
    }

}

