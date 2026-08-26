using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ClinicBookingApi.Core;
using ClinicBookingApi.DTO;
using ClinicBookingApi.Services;

namespace ClinicBookingApi.Controllers
{
	[ApiController]
	[Route("api/v1/patients")]
	public class PatientsController : ControllerBase
	{
		private readonly IApplicationService _applicationService;

		public PatientsController(IApplicationService applicationService)
		{
			_applicationService = applicationService;
		}

		/// <summary>
		/// Gets a patient by their ID.
		/// </summary>
		[HttpGet("{id:int}")]
		[Authorize(Policy = "VIEW_PATIENT")]
		[ProducesResponseType(typeof(PatientReadOnlyDTO), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<PatientReadOnlyDTO>> GetPatientById(int id)
		{
			var patient = await _applicationService.PatientService.GetPatientByIdAsync(id);
			return Ok(patient);
		}

		/// <summary>
		/// Gets a paginated list of patients.
		/// </summary>
		[HttpGet]
		[Authorize(Policy = "VIEW_PATIENTS")]
		[ProducesResponseType(typeof(PaginatedResult<PatientReadOnlyDTO>), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		public async Task<ActionResult<PaginatedResult<PatientReadOnlyDTO>>> GetPatients(
			[FromQuery] int pageNumber = 1,
			[FromQuery] int pageSize = 10)
		{
			var result = await _applicationService.PatientService
				.GetPaginatedPatientsAsync(pageNumber, pageSize);

			return Ok(result);
		}
	}
}