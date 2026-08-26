using ClinicBookingApi.Core;
using ClinicBookingApi.DTO;

namespace ClinicBookingApi.Services
{
	public interface IDoctorService
	{
		Task<UserReadOnlyDTO> SignUpUserAsync(DoctorSignupDTO request);
		Task<DoctorReadOnlyDTO> GetDoctorByIdAsync(int id);
		Task<PaginatedResult<DoctorReadOnlyDTO>> GetPaginatedDoctorsAsync(int pageNumber, int pageSize);
	}
}