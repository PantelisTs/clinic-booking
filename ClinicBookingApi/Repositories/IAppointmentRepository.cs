using ClinicBookingApi.Models;

namespace ClinicBookingApi.Repositories
{
	public interface IAppointmentRepository : IBaseRepository<Appointment>
	{
		Task<Patient?> GetAppointmentPatientAsync(int appointmentId);
		Task<Doctor?> GetAppointmentDoctorAsync(int appointmentId);
	}
}