using RanitSabitovKt_41_22.Database;
using RanitSabitovKt_41_22.Models;

namespace sabitov_ranit_maratovich_kt_41_22.Tests.Helpers
{
    public static class TestDataSeeder
    {
        public static async Task SeedAsync(UniversityDbContext context)
        {

            var departments = new List<Department>
            {
                new Department { DepartmentId = 1, Name = "ИТ" },
                new Department { DepartmentId = 2, Name = "Кафедра физики" }
            };

            var degrees = new List<AcademicDegree>
            {
                new AcademicDegree { AcademicDegreeId = 1, c_academic_degree_name = "Дандидат наук" },
                new AcademicDegree { AcademicDegreeId = 2, c_academic_degree_name = "Доктор наук" }
            };

            var staffers = new List<Staffer>
            {
                new Staffer { StafferId = 1, c_staff_name = "Доцент" },
                new Staffer { StafferId = 2, c_staff_name = "Профессор" }
            };

            await context.Departments.AddRangeAsync(departments);
            await context.AcademicDegrees.AddRangeAsync(degrees);
            await context.Staffers.AddRangeAsync(staffers);
            await context.SaveChangesAsync();

            var teachers = new List<Teacher>
            {
                new Teacher
                {
                    TeacherId = 1,
                    c_teacher_firstname = "Иван",
                    c_teacher_lastname = "Иванов",
                    c_teacher_middlename = "Иванович",
                    DepartmentId = departments[0].DepartmentId,
                    AcademicDegreeId = degrees[0].AcademicDegreeId,
                    StaffId = staffers[0].StafferId
                },
                new Teacher
                {
                    TeacherId = 2,
                    c_teacher_firstname = "Петр",
                    c_teacher_lastname = "Петров",
                    c_teacher_middlename = "Петрович",
                    DepartmentId = departments[1].DepartmentId,
                    AcademicDegreeId = degrees[0].AcademicDegreeId,
                    StaffId = staffers[0].StafferId
                },
                new Teacher
                {
                    TeacherId = 3,
                    c_teacher_firstname = "Сидоров",
                    c_teacher_lastname = "Сидоров",
                    c_teacher_middlename = "Сидорович",
                    DepartmentId = departments[1].DepartmentId,
                    AcademicDegreeId = degrees[1].AcademicDegreeId,
                    StaffId = staffers[1].StafferId
                }
            };

            await context.Teachers.AddRangeAsync(teachers);

            departments[0].HeadTeacherId = teachers[0].TeacherId;
            departments[1].HeadTeacherId = teachers[1].TeacherId;
            context.Departments.UpdateRange(departments);
            await context.SaveChangesAsync();

            // Дисциплины
            var disciplines = new List<Discipline>
            {
                new Discipline { DisciplineId = 1, c_discipline_name = "Программирование" },
                new Discipline { DisciplineId = 2, c_discipline_name = "Матем" }
            };
            await context.Disciplines.AddRangeAsync(disciplines);

            // Нагрузка
            var workloads = new List<Workload>
            {
                new Workload { WorkloadId = 1, TeacherId = teachers[0].TeacherId, DisciplineId = 1, c_workload_hours = 40 },
                new Workload { WorkloadId = 2, TeacherId = teachers[2].TeacherId, DisciplineId = 1, c_workload_hours = 35 },
                new Workload { WorkloadId = 3, TeacherId = teachers[2].TeacherId, DisciplineId = 2, c_workload_hours = 30 }
            };
            await context.Workloads.AddRangeAsync(workloads);
            await context.SaveChangesAsync();

        }
    }
}
