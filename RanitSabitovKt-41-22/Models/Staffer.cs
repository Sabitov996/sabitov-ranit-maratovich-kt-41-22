namespace RanitSabitovKt_41_22.Models
{
    public class Staffer
    {
        public int StafferId { get; set; } // <- имя класса + Id
        public string c_staff_name { get; set; }

        public ICollection<Teacher> Teachers { get; set; }
    }
}
