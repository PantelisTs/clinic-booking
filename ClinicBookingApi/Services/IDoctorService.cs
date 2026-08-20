using ClinicBookingApi.DTO;

namespace ClinicBookingApi.Services
{
	public interface IDoctorService
	{
		Task<UserReadOnlyDTO> SignUpUserAsync(DoctorSignupDTO request);
	}
}