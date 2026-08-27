using ClinicBookingApi.Core;
using ClinicBookingApi.Models;
using System.Linq.Expressions;

namespace ClinicBookingApi.Repositories
{
	public interface IPatientRepository : IBaseRepository<Patient>
	{
		Task<List<Appointment>> GetPatientAppointmentsAsync(int patientId);
		Task<PaginatedResult<User>> GetPaginatedUsersPatientsAsync(int pageNumber, int pageSize);
		Task<PaginatedResult<Patient>> GetPaginatedUsersPatientsFilteredAsync(int pageNumber, int pageSize,
			List<Expression<Func<Patient, bool>>> predicates);
		Task<Patient?> GetByIdWithUserAsync(int id);
		Task<PaginatedResult<Patient>> GetPaginatedPatientsWithUserAsync(int pageNumber, int pageSize);
		Task<Patient?> GetByUserIdAsync(int userId);
	}
}