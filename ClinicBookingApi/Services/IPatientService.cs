using ClinicBookingApi.Core;
using ClinicBookingApi.DTO;

namespace ClinicBookingApi.Services
{
	public interface IPatientService
	{
		Task<UserReadOnlyDTO> SignUpUserAsync(PatientSignupDTO request);
		Task<PatientReadOnlyDTO> GetPatientByIdAsync(int id);
		Task<PaginatedResult<PatientReadOnlyDTO>> GetPaginatedPatientsAsync(int pageNumber, int pageSize);
	}
}