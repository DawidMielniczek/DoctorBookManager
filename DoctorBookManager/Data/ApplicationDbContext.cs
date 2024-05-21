using DoctorBookManager.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DoctorBookManager.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<DoctorModel> Doctors { get; set; }
        public DbSet<DoctorReviewModel> DoctorReviews { get; set; }
        public DbSet<DoctorAppointmentModel> DoctorAppointmentModels { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<DoctorAppointmentModel>().HasIndex(i=>new {i.PatientId})
        }
    }
}