namespace ClinicBookingApi.Models
{
	public class User : BaseEntity
	{
		public int Id { get; set; }
		public string Username { get; set; } = null!;
		public string Email { get; set; } = null!;
		public string Password { get; set; } = null!;
		public string FirstName { get; set; } = null!;
		public string LastName { get; set; } = null!;

		public int RoleId { get; set; }
		public Role Role { get; set; } = null!;

		public Patient? Patient { get; set; }
		public Doctor? Doctor { get; set; }
	}
}