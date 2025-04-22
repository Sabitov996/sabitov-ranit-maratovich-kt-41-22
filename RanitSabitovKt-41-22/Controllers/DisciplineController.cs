using Microsoft.AspNetCore.Mvc;
using RanitSabitovKt_41_22.Database;
using RanitSabitovKt_41_22.Filters;
using RanitSabitovKt_41_22.Interfaces;
using RanitSabitovKt_41_22.Models;

namespace RanitSabitovKt_41_22.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DisciplineController : ControllerBase
    {
        private readonly IDisciplineService _disciplineService;
        private readonly UniversityDbContext _dbContext;

        public DisciplineController(IDisciplineService disciplineService, UniversityDbContext dbContext)
        {
            _disciplineService = disciplineService;
            _dbContext = dbContext;
        }

        [HttpPost("filter")]
        public async Task<IActionResult> GetDisciplinesAsync(DisciplineFilter filter, CancellationToken cancellationToken)
        {
            var disciplines = await _disciplineService.GetDisciplinesAsync(filter, cancellationToken);
            return Ok(disciplines);
        }

        // Добавление дисциплины
        [HttpPost("add")]
        public async Task<IActionResult> AddDiscipline([FromBody] DisciplineCreateDto dto)
        {
            var discipline = new Discipline
            {
                c_discipline_name = dto.C_Discipline_Name
            };

            _dbContext.Disciplines.Add(discipline);
            await _dbContext.SaveChangesAsync();
            return Ok(discipline);
        }
        //  Изменение дисциплины
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateDiscipline(int id, [FromBody] DisciplineUpdateDto dto)
        {
            var discipline = await _dbContext.Disciplines.FindAsync(id);
            if (discipline == null)
                return NotFound();

            discipline.c_discipline_name = dto.C_Discipline_Name;

            await _dbContext.SaveChangesAsync();
            return Ok(discipline);
        }

        //  Удаление дисциплины
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteDiscipline(int id)
        {
            var discipline = await _dbContext.Disciplines.FindAsync(id);
            if (discipline == null)
                return NotFound();

            _dbContext.Disciplines.Remove(discipline);
            await _dbContext.SaveChangesAsync();

            return Ok($"Дисциплина (ID = {id}) успешно удалена");
        }
    }
}
