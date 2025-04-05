namespace RanitSabitovKt_41_22.Models
{
    public class Teacher
    {
        public int TeacherId { get; set; }
        public string c_teacher_firstname { get; set; }
        public string c_teacher_lastname { get; set; }
        public string c_teacher_middlename { get; set; }

        public int AcademicDegreeId { get; set; }
        public AcademicDegree AcademicDegree { get; set; }

        public int StaffId { get; set; }
        public Staffer Staff { get; set; }

        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        public ICollection<Workload> Workloads { get; set; }
    }
}
