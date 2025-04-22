using Microsoft.EntityFrameworkCore;
using RanitSabitovKt_41_22.Models;

namespace RanitSabitovKt_41_22.Database
{
    public class UniversityDbContext : DbContext
    {
        public UniversityDbContext(DbContextOptions<UniversityDbContext> options)
            : base(options)
        {
        }

        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<AcademicDegree> AcademicDegrees { get; set; }
        public DbSet<Staffer> Staffers { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Workload> Workloads { get; set; }
        public DbSet<Discipline> Disciplines { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Определение связей и внешних ключей
            modelBuilder.Entity<Teacher>()
                .HasOne(t => t.AcademicDegree)
                .WithMany(a => a.Teachers)
                .HasForeignKey(t => t.AcademicDegreeId);

            modelBuilder.Entity<Teacher>()
                .HasOne(t => t.Staff)
                .WithMany(s => s.Teachers)
                .HasForeignKey(t => t.StaffId);

            modelBuilder.Entity<Teacher>()
                .HasOne(t => t.Department)
                .WithMany(d => d.Teachers)
                .HasForeignKey(t => t.DepartmentId);

            modelBuilder.Entity<Workload>()
                .HasOne(w => w.Teacher)
                .WithMany(t => t.Workloads)
                .HasForeignKey(w => w.TeacherId);

            modelBuilder.Entity<Workload>()
                .HasOne(w => w.Discipline)
                .WithMany(d => d.Workloads)
                .HasForeignKey(w => w.DisciplineId);

            modelBuilder.Entity<Department>()
                .HasOne(d => d.HeadTeacher)
                .WithMany()
                .HasForeignKey(d => d.HeadTeacherId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Teacher>()
                .HasOne(t => t.Department)
                .WithMany(d => d.Teachers)
                .HasForeignKey(t => t.DepartmentId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
