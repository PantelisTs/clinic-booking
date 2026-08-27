using System.ComponentModel.DataAnnotations;

namespace ClinicBookingApi.DTO
{
	public record AppointmentCreateDTO
	{
		[Required(ErrorMessage = "The {0} field is required.")]
		public int? DoctorId { get; set; }

		[Required(ErrorMessage = "The {0} field is required.")]
		public DateTime? ScheduledAt { get; set; }
	}
}