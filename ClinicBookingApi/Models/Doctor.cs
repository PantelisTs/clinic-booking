namespace ClinicBookingApi.Models
{
	public class Doctor : BaseEntity
	{
		public int Id { get; set; }
		public string Specialty { get; set; } = null!;

		public int UserId { get; set; }
		public User User { get; set; } = null!;

		public ICollection<Appointment> Appointments { get; set; } = new HashSet<Appointment>();
	}
}