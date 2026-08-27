using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ClinicBookingApi.Core;
using ClinicBookingApi.DTO;
using ClinicBookingApi.Services;
using System.Security.Claims;

namespace ClinicBookingApi.Controllers
{
	[ApiController]
	[Route("api/v1/appointments")]
	public class AppointmentsController : ControllerBase
	{
		private readonly IApplicationService _applicationService;

		public AppointmentsController(IApplicationService applicationService)
		{
			_applicationService = applicationService;
		}

		/// <summary>
		/// Creates a new appointment for the current patient.
		/// </summary>
		[HttpPost]
		[Authorize(Policy = "INSERT_APPOINTMENT")]
		[ProducesResponseType(typeof(AppointmentReadOnlyDTO), StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status409Conflict)]
		public async Task<ActionResult<AppointmentReadOnlyDTO>> CreateAppointment(
			[FromBody] AppointmentCreateDTO request)
		{
			var currentUserId = GetCurrentUserId();
			var appointment = await _applicationService.AppointmentService
				.CreateAppointmentAsync(currentUserId, request);

			return CreatedAtAction(
				actionName: nameof(GetAppointmentById),
				routeValues: new { id = appointment.Id },
				value: appointment);
		}

		/// <summary>
		/// Gets an appointment by its ID.
		/// </summary>
		[HttpGet("{id:int}")]
		[Authorize]
		[ProducesResponseType(typeof(AppointmentReadOnlyDTO), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<AppointmentReadOnlyDTO>> GetAppointmentById(int id)
		{
			var currentUserId = GetCurrentUserId();
			var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;

			var appointment = await _applicationService.AppointmentService
				.GetAppointmentByIdAsync(id, currentUserId, currentUserRole);

			return Ok(appointment);
		}

		/// <summary>
		/// Gets a paginated list of all appointments.
		/// </summary>
		[HttpGet]
		[Authorize(Policy = "VIEW_APPOINTMENTS")]
		[ProducesResponseType(typeof(PaginatedResult<AppointmentReadOnlyDTO>), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		public async Task<ActionResult<PaginatedResult<AppointmentReadOnlyDTO>>> GetAppointments(
			[FromQuery] int pageNumber = 1,
			[FromQuery] int pageSize = 10)
		{
			var result = await _applicationService.AppointmentService
				.GetPaginatedAppointmentsAsync(pageNumber, pageSize);

			return Ok(result);
		}

		/// <summary>
		/// Updates an appointment's notes and/or status (doctor only, own appointments).
		/// </summary>
		[HttpPut("{id:int}")]
		[Authorize(Policy = "EDIT_APPOINTMENT")]
		[ProducesResponseType(typeof(AppointmentReadOnlyDTO), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<AppointmentReadOnlyDTO>> UpdateAppointment(
			int id, [FromBody] AppointmentUpdateDTO request)
		{
			var currentUserId = GetCurrentUserId();
			var appointment = await _applicationService.AppointmentService
				.UpdateAppointmentAsync(id, currentUserId, request.Notes, request.Status);

			return Ok(appointment);
		}

		/// <summary>
		/// Cancels an appointment (patient only, own appointment).
		/// </summary>
		[HttpPatch("{id:int}/cancel")]
		[Authorize(Policy = "CANCEL_APPOINTMENT")]
		[ProducesResponseType(typeof(AppointmentReadOnlyDTO), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<AppointmentReadOnlyDTO>> CancelAppointment(int id)
		{
			var currentUserId = GetCurrentUserId();
			var appointment = await _applicationService.AppointmentService
				.CancelAppointmentAsync(id, currentUserId);

			return Ok(appointment);
		}

		private int GetCurrentUserId()
		{
			return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
		}
	}
}