using ClinicBookingApi.Core;
using ClinicBookingApi.DTO;

namespace ClinicBookingApi.Services
{
	public interface IAppointmentService
	{
		Task<AppointmentReadOnlyDTO> CreateAppointmentAsync(int currentUserId, AppointmentCreateDTO request);
		Task<AppointmentReadOnlyDTO> GetAppointmentByIdAsync(int id);
		Task<PaginatedResult<AppointmentReadOnlyDTO>> GetPaginatedAppointmentsAsync(int pageNumber, int pageSize);
		Task<AppointmentReadOnlyDTO> UpdateAppointmentAsync(int id, int currentUserId, string? notes, string? status);
		Task<AppointmentReadOnlyDTO> CancelAppointmentAsync(int id, int currentUserId);
	}
}