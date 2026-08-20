using ClinicBookingApi.DTO;

namespace ClinicBookingApi.Services
{
	public interface IPatientService
	{
		Task<UserReadOnlyDTO> SignUpUserAsync(PatientSignupDTO request);
	}
}