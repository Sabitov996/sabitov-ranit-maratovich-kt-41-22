using Microsoft.AspNetCore.Mvc;
using RanitSabitovKt_41_22.Filters;
using RanitSabitovKt_41_22.Interfaces;

namespace RanitSabitovKt_41_22.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TeacherController : ControllerBase
    {
        private readonly ITeacherService _teacherService;

        public TeacherController(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        [HttpPost(Name = "GetTeachersByFilter")]
        public async Task<IActionResult> GetTeachersByFilterAsync(TeacherFilter filter, CancellationToken cancellationToken = default)
        {
            var teachers = await _teacherService.GetTeachersByFilterAsync(filter, cancellationToken);
            return Ok(teachers);
        }
    }
}
