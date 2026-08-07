using Microsoft.EntityFrameworkCore;
using ClinicBookingApi.Core;
using ClinicBookingApi.Data;
using ClinicBookingApi.Models;
using System.Linq.Expressions;

namespace ClinicBookingApi.Repositories
{
	public class PatientRepository : BaseRepository<Patient>, IPatientRepository
	{
		public PatientRepository(ClinicBookingDbContext context) : base(context)
		{
		}

		public async Task<PaginatedResult<User>> GetPaginatedUsersPatientsAsync(int pageNumber, int pageSize)
		{
			int skip = (pageNumber - 1) * pageSize;

			var usersWithRolePatient = await _context.Users
				.Include(u => u.Patient) // Eager loading της σχετικής οντότητας Patient
				.Where(u => u.Patient != null)
				.OrderBy(u => u.Id)
				.Skip(skip)
				.Take(pageSize)
				.ToListAsync();

			int totalRecords = await _context.Users
				.Where(u => u.Patient != null)
				.CountAsync();

			return new PaginatedResult<User>(usersWithRolePatient, totalRecords, pageNumber, pageSize);
		}

		public async Task<PaginatedResult<Patient>> GetPaginatedUsersPatientsFilteredAsync(int pageNumber,
			int pageSize, List<Expression<Func<Patient, bool>>> predicates)
		{
			IQueryable<Patient> query = _context.Patients;

			// Apply predicates as Expression<Func<Patient, bool>> so they run in DB
			if (predicates != null && predicates.Count > 0)
			{
				foreach (var predicate in predicates)
				{
					query = query.Where(predicate);
				}
			}

			// Get total count BEFORE pagination
			int totalRecords = await query.CountAsync();

			// Paginate AFTER filtering
			int skip = (pageNumber - 1) * pageSize;

			var data = await query
				.OrderBy(p => p.Id)
				.Skip(skip)
				.Take(pageSize)
				.ToListAsync();

			return new PaginatedResult<Patient>
			{
				Data = data,
				TotalRecords = totalRecords,
				PageNumber = pageNumber,
				PageSize = pageSize
			};
		}

		public async Task<List<Appointment>> GetPatientAppointmentsAsync(int patientId)
		{
			List<Appointment> appointments;

			appointments = await _context.Patients
				.Where(p => p.Id == patientId)
				.SelectMany(p => p.Appointments)
				.ToListAsync();

			return appointments;
		}
	}
}