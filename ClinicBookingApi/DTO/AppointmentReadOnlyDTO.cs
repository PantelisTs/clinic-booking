namespace ClinicBookingApi.DTO
{
	public record AppointmentReadOnlyDTO
	{
		public int Id { get; set; }
		public DateTime ScheduledAt { get; set; }
		public string Status { get; set; } = null!;
		public string? Notes { get; set; }
		public int PatientId { get; set; }
		public string PatientName { get; set; } = null!;
		public int DoctorId { get; set; }
		public string DoctorName { get; set; } = null!;
	}
}