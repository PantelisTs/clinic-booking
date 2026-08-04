namespace ClinicBookingApi.Models
{
	public class Appointment : BaseEntity
	{
		public int Id { get; set; }
		public DateTime ScheduledAt { get; set; }
		public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;
		public string? Notes { get; set; }

		public int PatientId { get; set; }
		public Patient Patient { get; set; } = null!;

		public int DoctorId { get; set; }
		public Doctor Doctor { get; set; } = null!;
	}

	public enum AppointmentStatus
	{
		Pending,
		Confirmed,
		Cancelled,
		Completed
	}
}