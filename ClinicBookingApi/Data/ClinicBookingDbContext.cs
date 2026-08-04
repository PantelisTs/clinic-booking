using Microsoft.EntityFrameworkCore;
using ClinicBookingApi.Models;

namespace ClinicBookingApi.Data
{
	public class ClinicBookingDbContext : DbContext
	{
		public ClinicBookingDbContext(DbContextOptions<ClinicBookingDbContext> options)
			: base(options)
		{
		}

		public DbSet<User> Users { get; set; }
		public DbSet<Role> Roles { get; set; }
		public DbSet<Capability> Capabilities { get; set; }
		public DbSet<Patient> Patients { get; set; }
		public DbSet<Doctor> Doctors { get; set; }
		public DbSet<Appointment> Appointments { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Capability>(entity =>
			{
				entity.Property(e => e.Name).HasMaxLength(100);
				entity.Property(e => e.Description).HasMaxLength(255);
				entity.HasIndex(e => e.Name, "UQ_Capabilities_Name").IsUnique();
			});

			modelBuilder.Entity<Role>(entity =>
			{
				entity.Property(e => e.Name).HasMaxLength(50);
				entity.HasIndex(e => e.Name, "UQ_Roles_Name").IsUnique();

				entity.HasMany(d => d.Capabilities).WithMany(p => p.Roles)
					.UsingEntity("RolesCapabilities");
			});

			modelBuilder.Entity<User>(entity =>
			{
				entity.Property(e => e.Username).HasMaxLength(50);
				entity.Property(e => e.Email).HasMaxLength(50);
				entity.Property(e => e.Password).HasMaxLength(60);
				entity.Property(e => e.FirstName).HasMaxLength(50);
				entity.Property(e => e.LastName).HasMaxLength(50);

				entity.HasOne(d => d.Role).WithMany(p => p.Users)
					.HasForeignKey(d => d.RoleId)
					.OnDelete(DeleteBehavior.Restrict)
					.HasConstraintName("FK_Users_RoleId");

				entity.HasIndex(e => e.Username, "IX_Users_Username").IsUnique();
				entity.HasIndex(e => e.Email, "IX_Users_Email").IsUnique();
				entity.HasIndex(e => e.RoleId, "IX_Users_RoleId");
			});

			modelBuilder.Entity<Patient>(entity =>
			{
				entity.HasOne(d => d.User).WithOne(p => p.Patient)
					.HasForeignKey<Patient>(d => d.UserId)
					.OnDelete(DeleteBehavior.Cascade)
					.HasConstraintName("FK_Patients_UserId");

				entity.HasIndex(e => e.UserId, "IX_Patients_UserId").IsUnique();
			});

			modelBuilder.Entity<Doctor>(entity =>
			{
				entity.Property(e => e.Specialty).HasMaxLength(100);

				entity.HasOne(d => d.User).WithOne(p => p.Doctor)
					.HasForeignKey<Doctor>(d => d.UserId)
					.OnDelete(DeleteBehavior.Cascade)
					.HasConstraintName("FK_Doctors_UserId");

				entity.HasIndex(e => e.UserId, "IX_Doctors_UserId").IsUnique();
			});

			modelBuilder.Entity<Appointment>(entity =>
			{
				entity.Property(e => e.Notes).HasMaxLength(500);

				entity.HasOne(d => d.Patient).WithMany(p => p.Appointments)
					.HasForeignKey(d => d.PatientId)
					.OnDelete(DeleteBehavior.Restrict)
					.HasConstraintName("FK_Appointments_PatientId");

				entity.HasOne(d => d.Doctor).WithMany(p => p.Appointments)
					.HasForeignKey(d => d.DoctorId)
					.OnDelete(DeleteBehavior.Restrict)
					.HasConstraintName("FK_Appointments_DoctorId");

				entity.HasIndex(e => e.PatientId, "IX_Appointments_PatientId");
				entity.HasIndex(e => e.DoctorId, "IX_Appointments_DoctorId");
			});
		}
	}
}