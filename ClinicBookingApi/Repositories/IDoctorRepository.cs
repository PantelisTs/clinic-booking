using ClinicBookingApi.Core;
using ClinicBookingApi.Models;
using System.Linq.Expressions;

namespace ClinicBookingApi.Repositories
{
	public interface IDoctorRepository : IBaseRepository<Doctor>
	{
		Task<List<Appointment>> GetDoctorAppointmentsAsync(int doctorId);
		Task<User?> GetUserDoctorByUsernameAsync(string username);
		Task<PaginatedResult<User>> GetPaginatedDoctorsAsync(int pageNumber, int pageSize,
			List<Expression<Func<User, bool>>> predicates);
	}
}