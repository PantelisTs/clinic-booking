namespace ClinicBookingApi.Models
{
	public class Patient : BaseEntity
	{
		public int Id { get; set; }

		public int UserId { get; set; }
		public User User { get; set; } = null!;

		public ICollection<Appointment> Appointments { get; set; } = new HashSet<Appointment>();
	}
}