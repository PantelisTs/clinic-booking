using ClinicBookingApi.Core;
using ClinicBookingApi.Data;
using ClinicBookingApi.Models;
using Microsoft.EntityFrameworkCore;

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
		public async Task<bool> HasConflictAsync(int doctorId, DateTime scheduledAt)
		{
			return await _context.Appointments
				.AnyAsync(a => a.DoctorId == doctorId
							 && a.ScheduledAt == scheduledAt
							 && a.Status != AppointmentStatus.Cancelled);
		}

		public async Task<Appointment?> GetByIdWithDetailsAsync(int id)
		{
			return await _context.Appointments
				.Include(a => a.Patient).ThenInclude(p => p.User)
				.Include(a => a.Doctor).ThenInclude(d => d.User)
				.FirstOrDefaultAsync(a => a.Id == id);
		}

		public async Task<PaginatedResult<Appointment>> GetPaginatedWithDetailsAsync(int pageNumber, int pageSize)
		{
			int totalRecords = await _context.Appointments.CountAsync();
			int skip = (pageNumber - 1) * pageSize;

			var data = await _context.Appointments
				.Include(a => a.Patient).ThenInclude(p => p.User)
				.Include(a => a.Doctor).ThenInclude(d => d.User)
				.OrderBy(a => a.Id)
				.Skip(skip)
				.Take(pageSize)
				.ToListAsync();

			return new PaginatedResult<Appointment>()
			{
				Data = data,
				TotalRecords = totalRecords,
				PageNumber = pageNumber,
				PageSize = pageSize
			};
		}
	}
}