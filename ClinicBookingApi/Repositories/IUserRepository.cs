using ClinicBookingApi.Core;
using ClinicBookingApi.Models;
using System.Linq.Expressions;

namespace ClinicBookingApi.Repositories
{
	public interface IUserRepository : IBaseRepository<User>
	{
		Task<User?> GetUserByUsernameAsync(string username);
		Task<PaginatedResult<User>> GetUsersAsync(int pageNumber, int pageSize,
			List<Expression<Func<User, bool>>> predicates);
	}
}