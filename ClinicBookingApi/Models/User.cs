using System.Numerics;

namespace ClinicBookingApi.Models
{
	public class User : BaseEntity
	{
		public int Id { get; set; }
		public string Username { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;
		public string Password { get; set; } = string.Empty;
		public string FirstName { get; set; } = string.Empty;
		public string LastName { get; set; } = string.Empty;

		public int RoleId { get; set; }
		public Role Role { get; set; } = null!;

		public Patient? Patient { get; set; }
		public Doctor? Doctor { get; set; }
	}
}