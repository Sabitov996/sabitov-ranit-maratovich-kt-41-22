namespace RanitSabitovKt_41_22.Models
{
    public class Discipline
    {
        public int DisciplineId { get; set; }
        public string c_discipline_name { get; set; }

        public ICollection<Workload> Workloads { get; set; }
    }
}
