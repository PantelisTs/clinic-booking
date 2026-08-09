using Microsoft.EntityFrameworkCore;
using ClinicBookingApi.Core;
using ClinicBookingApi.Data;
using ClinicBookingApi.Models;
using System.Linq.Expressions;

namespace ClinicBookingApi.Repositories
{
	public class DoctorRepository : BaseRepository<Doctor>, IDoctorRepository
	{
		public DoctorRepository(ClinicBookingDbContext context) : base(context)
		{
		}

		public async Task<User?> GetUserDoctorByUsernameAsync(string username)
		{
			var userDoctor = await _context.Users
				.Include(u => u.Doctor) // Eager loading like Join in SQL
				.Where(u => u.Username == username && u.Doctor != null)
				.SingleOrDefaultAsync();    // fetches 0 or 1 results, throws Exception

			return userDoctor;
		}

		public async Task<PaginatedResult<User>> GetPaginatedDoctorsAsync(int pageNumber, int pageSize,
			List<Expression<Func<User, bool>>> predicates)
		{
			int totalRecords;
			IQueryable<User> query = _context.Users
				.Include(u => u.Doctor)
				.Where(u => u.Doctor != null); // Φιλτράρουμε μόνο τους χρήστες που είναι doctors

			if (predicates != null && predicates.Count > 0)
			{
				foreach (var predicate in predicates)
				{
					query = query.Where(predicate); // υπονοείται το AND
				}
			}

			totalRecords = await query.CountAsync();
			int skip = (pageNumber - 1) * pageSize;

			var data = await query
				.OrderBy(u => u.Id) // Πάντα OrderBy για να διασφαλίσουμε την σταθερή σειρά των αποτελεσμάτων
				.Skip(skip)
				.Take(pageSize)
				.ToListAsync();

			return new PaginatedResult<User>()
			{
				Data = data,
				TotalRecords = totalRecords,
				PageNumber = pageNumber,
				PageSize = pageSize
			};
		}

		public async Task<List<Appointment>> GetDoctorAppointmentsAsync(int doctorId)
		{
			List<Appointment> appointments;

			appointments = await _context.Appointments
				.Where(a => a.DoctorId == doctorId)
				.ToListAsync();

			return appointments;
		}
	}
}