namespace ClinicBookingApi.DTO
{
	public record AppointmentUpdateDTO
	{
		public string? Notes { get; set; }
		public string? Status { get; set; }
	}
}