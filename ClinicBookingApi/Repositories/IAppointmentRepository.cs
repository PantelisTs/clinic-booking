using ClinicBookingApi.Core;
using ClinicBookingApi.Models;

namespace ClinicBookingApi.Repositories
{
	public interface IAppointmentRepository : IBaseRepository<Appointment>
	{
		Task<Patient?> GetAppointmentPatientAsync(int appointmentId);
		Task<Doctor?> GetAppointmentDoctorAsync(int appointmentId);
		Task<bool> HasConflictAsync(int doctorId, DateTime scheduledAt);
		Task<Appointment?> GetByIdWithDetailsAsync(int id);
		Task<PaginatedResult<Appointment>> GetPaginatedWithDetailsAsync(int pageNumber, int pageSize);
	}
}