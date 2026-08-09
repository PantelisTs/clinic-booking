using Microsoft.EntityFrameworkCore;
using ClinicBookingApi.Data;
using ClinicBookingApi.Models;

namespace ClinicBookingApi.Repositories
{
	public class AppointmentRepository : BaseRepository<Appointment>, IAppointmentRepository
	{
		public AppointmentRepository(ClinicBookingDbContext context) : base(context)
		{
		}

		public async Task<Patient?> GetAppointmentPatientAsync(int appointmentId)
		{
			var appointment = await _context.Appointments
					.Include(a => a.Patient) // eagerly loads related entities in the same query
					.FirstOrDefaultAsync(a => a.Id == appointmentId);

			return appointment?.Patient; // not second query, since patient has loaded
		}

		public async Task<Doctor?> GetAppointmentDoctorAsync(int appointmentId)
		{
			var appointment = await _context.Appointments
					.Include(a => a.Doctor) // eagerly loads related entities in the same query
					.FirstOrDefaultAsync(a => a.Id == appointmentId);

			return appointment?.Doctor; // not second query, since doctor has loaded
		}
	}
}