using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ClinicBookingApi.Core;
using ClinicBookingApi.DTO;
using ClinicBookingApi.Services;

namespace ClinicBookingApi.Controllers
{
	[ApiController]
	[Route("api/v1/doctors")]
	public class DoctorsController : ControllerBase
	{
		private readonly IApplicationService _applicationService;

		public DoctorsController(IApplicationService applicationService)
		{
			_applicationService = applicationService;
		}

		/// <summary>
		/// Gets a doctor by their ID.
		/// </summary>
		/// <param name="id">The doctor ID.</param>
		/// <returns>The doctor details.</returns>
		/// <response code="200">Returns the requested doctor.</response>
		/// <response code="401">If the request is not authenticated.</response>
		/// <response code="404">If no doctor exists with the given ID.</response>
		[HttpGet("{id:int}")]
		[Authorize(Policy = "VIEW_DOCTORS")]
		[ProducesResponseType(typeof(DoctorReadOnlyDTO), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<DoctorReadOnlyDTO>> GetDoctorById(int id)
		{
			var doctor = await _applicationService.DoctorService.GetDoctorByIdAsync(id);
			return Ok(doctor);
		}

		/// <summary>
		/// Gets a paginated list of doctors.
		/// </summary>
		/// <param name="pageNumber">The page number (1-based). Default is 1.</param>
		/// <param name="pageSize">The number of items per page. Default is 10.</param>
		/// <returns>A paginated list of doctors.</returns>
		/// <response code="200">Returns the paginated doctor list.</response>
		/// <response code="401">If the request is not authenticated.</response>
		[HttpGet]
		[Authorize(Policy = "VIEW_DOCTORS")]
		[ProducesResponseType(typeof(PaginatedResult<DoctorReadOnlyDTO>), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public async Task<ActionResult<PaginatedResult<DoctorReadOnlyDTO>>> GetDoctors(
			[FromQuery] int pageNumber = 1,
			[FromQuery] int pageSize = 10)
		{
			var result = await _applicationService.DoctorService
				.GetPaginatedDoctorsAsync(pageNumber, pageSize);

			return Ok(result);
		}
	}
}