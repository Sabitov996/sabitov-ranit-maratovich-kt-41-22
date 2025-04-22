namespace RanitSabitovKt_41_22.Models
{
    public class Department
    {
        public int DepartmentId { get; set; }
        public string Name { get; set; }
        public int HeadTeacherId { get; set; }

        public DateTime FoundedDate { get; set; } 

        public Teacher HeadTeacher { get; set; }  // ⬅️ для Include(d => d.HeadTeacher)
        public ICollection<Teacher> Teachers { get; set; }
    }
}
