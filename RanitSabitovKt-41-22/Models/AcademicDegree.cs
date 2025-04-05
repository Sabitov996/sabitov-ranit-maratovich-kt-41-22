namespace RanitSabitovKt_41_22.Models
{
    public class AcademicDegree
    {
        public int AcademicDegreeId { get; set; }
        public string c_academic_degree_name { get; set; }

        public ICollection<Teacher> Teachers { get; set; }
    }
}
