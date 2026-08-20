using ClinicBookingApi.Core;
using ClinicBookingApi.Core.Filters;
using ClinicBookingApi.DTO;
using ClinicBookingApi.Models;

namespace ClinicBookingApi.Services
{
	public interface IUserService
	{
		Task<User> VerifyAndGetUserAsync(UserLoginDTO credentials);
		Task<UserReadOnlyDTO> GetUserByUsernameAsync(string username);
		Task<UserReadOnlyDTO> GetUserByIdAsync(int id);
		Task<PaginatedResult<UserReadOnlyDTO>> GetPaginatedUsersFilteredAsync(int pageNumber,
			int pageSize, UserFiltersDTO userFiltersDTO);
		string CreateUserToken(User user);
	}
}