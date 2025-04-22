using Microsoft.AspNetCore.Mvc;
using RanitSabitovKt_41_22.Database;
using RanitSabitovKt_41_22.Filters;
using RanitSabitovKt_41_22.Interfaces;
using RanitSabitovKt_41_22.Models;

namespace RanitSabitovKt_41_22.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TeacherController : ControllerBase
    {
        private readonly ITeacherService _teacherService;
        private readonly UniversityDbContext _dbContext;

        public TeacherController(ITeacherService teacherService, UniversityDbContext dbContext)
        {
            _teacherService = teacherService;
            _dbContext = dbContext;
        }

        // Получение преподавателей с фильтрацией
        [HttpPost("filter")]
        public async Task<IActionResult> GetTeachersAsync(TeacherFilter filter, CancellationToken cancellationToken)
        {
            var teachers = await _teacherService.GetTeachersAsync(filter, cancellationToken);
            return Ok(teachers);
        }

        // добавление преподавателя
        [HttpPost("add")]
        public async Task<IActionResult> AddTeacher([FromBody] TeacherCreateDto dto)
        {
            var teacher = new Teacher
            {
                c_teacher_firstname = dto.C_Teacher_Firstname,
                c_teacher_lastname = dto.C_Teacher_Lastname,
                c_teacher_middlename = dto.C_Teacher_Middlename,
                AcademicDegreeId = dto.AcademicDegreeId,
                StaffId = dto.StaffId,
                DepartmentId = dto.DepartmentId
            };

            _dbContext.Teachers.Add(teacher);
            await _dbContext.SaveChangesAsync();
            return Ok(teacher);
        }
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateTeacher(int id, [FromBody] TeacherUpdateDto dto)
        {
            var teacher = await _dbContext.Teachers.FindAsync(id);
            if (teacher == null)
                return NotFound();

            teacher.c_teacher_firstname = dto.C_Teacher_Firstname;
            teacher.c_teacher_lastname = dto.C_Teacher_Lastname;
            teacher.c_teacher_middlename = dto.C_Teacher_Middlename;
            teacher.AcademicDegreeId = dto.AcademicDegreeId;
            teacher.StaffId = dto.StaffId;
            teacher.DepartmentId = dto.DepartmentId;

            await _dbContext.SaveChangesAsync();
            return Ok(teacher);
        }
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteTeacher(int id)
        {
            var teacher = await _dbContext.Teachers.FindAsync(id);
            if (teacher == null)
                return NotFound();

            _dbContext.Teachers.Remove(teacher);
            await _dbContext.SaveChangesAsync();

            return Ok($"Преподаватель (ID = {id}) успешно удалён");
        }
    }
}
