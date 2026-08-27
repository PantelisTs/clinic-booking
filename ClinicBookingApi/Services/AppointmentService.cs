using AutoMapper;
using ClinicBookingApi.Core;
using ClinicBookingApi.DTO;
using ClinicBookingApi.Exceptions;
using ClinicBookingApi.Models;
using ClinicBookingApi.Repositories;

namespace ClinicBookingApi.Services
{
	public class AppointmentService : IAppointmentService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly ILogger<AppointmentService> _logger;

		public AppointmentService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<AppointmentService> logger)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_logger = logger;
		}

		public async Task<AppointmentReadOnlyDTO> CreateAppointmentAsync(int currentUserId, AppointmentCreateDTO request)
		{
			var patient = await _unitOfWork.PatientRepository.GetByUserIdAsync(currentUserId)
				?? throw new EntityNotFoundException("Patient", "No patient profile found for the current user");

			var doctor = await _unitOfWork.DoctorRepository.GetByIdAsync(request.DoctorId!.Value)
				?? throw new EntityNotFoundException("Doctor", $"Doctor with id {request.DoctorId} not found");

			if (request.ScheduledAt!.Value <= DateTime.UtcNow)
			{
				throw new InvalidArgumentException("Appointment", "Appointment date must be in the future");
			}

			var hasConflict = await _unitOfWork.AppointmentRepository
				.HasConflictAsync(doctor.Id, request.ScheduledAt.Value);

			if (hasConflict)
			{
				throw new EntityAlreadyExistsException("Appointment", "This doctor already has an appointment at this time");
			}

			var appointment = new Appointment
			{
				PatientId = patient.Id,
				DoctorId = doctor.Id,
				ScheduledAt = request.ScheduledAt.Value,
				Status = AppointmentStatus.Pending
			};

			await _unitOfWork.AppointmentRepository.AddAsync(appointment);
			await _unitOfWork.SaveAsync();

			var created = await _unitOfWork.AppointmentRepository.GetByIdWithDetailsAsync(appointment.Id);

			_logger.LogInformation("Appointment {Id} created for patient {PatientId} with doctor {DoctorId}",
				appointment.Id, patient.Id, doctor.Id);

			return _mapper.Map<AppointmentReadOnlyDTO>(created);
		}

		public async Task<AppointmentReadOnlyDTO> GetAppointmentByIdAsync(int id)
		{
			var appointment = await _unitOfWork.AppointmentRepository.GetByIdWithDetailsAsync(id)
				?? throw new EntityNotFoundException("Appointment", $"Appointment with id {id} not found");

			return _mapper.Map<AppointmentReadOnlyDTO>(appointment);
		}

		public async Task<PaginatedResult<AppointmentReadOnlyDTO>> GetPaginatedAppointmentsAsync(int pageNumber, int pageSize)
		{
			var result = await _unitOfWork.AppointmentRepository.GetPaginatedWithDetailsAsync(pageNumber, pageSize);

			var dtoResult = new PaginatedResult<AppointmentReadOnlyDTO>()
			{
				Data = _mapper.Map<List<AppointmentReadOnlyDTO>>(result.Data),
				TotalRecords = result.TotalRecords,
				PageNumber = result.PageNumber,
				PageSize = result.PageSize
			};

			_logger.LogInformation("Retrieved {Count} appointments", dtoResult.Data.Count);
			return dtoResult;
		}

		public async Task<AppointmentReadOnlyDTO> UpdateAppointmentAsync(int id, int currentUserId, string? notes, string? status)
		{
			var appointment = await _unitOfWork.AppointmentRepository.GetByIdWithDetailsAsync(id)
				?? throw new EntityNotFoundException("Appointment", $"Appointment with id {id} not found");

			if (appointment.Doctor.UserId != currentUserId)
			{
				throw new EntityForbiddenException("Appointment", "You can only edit your own appointments");
			}

			if (!string.IsNullOrWhiteSpace(notes))
			{
				appointment.Notes = notes;
			}

			if (!string.IsNullOrWhiteSpace(status) && Enum.TryParse<AppointmentStatus>(status, true, out var parsedStatus))
			{
				appointment.Status = parsedStatus;
			}

			await _unitOfWork.AppointmentRepository.UpdateAsync(appointment);
			await _unitOfWork.SaveAsync();

			_logger.LogInformation("Appointment {Id} updated by doctor user {UserId}", id, currentUserId);
			return _mapper.Map<AppointmentReadOnlyDTO>(appointment);
		}

		public async Task<AppointmentReadOnlyDTO> CancelAppointmentAsync(int id, int currentUserId)
		{
			var appointment = await _unitOfWork.AppointmentRepository.GetByIdWithDetailsAsync(id)
				?? throw new EntityNotFoundException("Appointment", $"Appointment with id {id} not found");

			if (appointment.Patient.UserId != currentUserId)
			{
				throw new EntityForbiddenException("Appointment", "You can only cancel your own appointments");
			}

			appointment.Status = AppointmentStatus.Cancelled;

			await _unitOfWork.AppointmentRepository.UpdateAsync(appointment);
			await _unitOfWork.SaveAsync();

			_logger.LogInformation("Appointment {Id} cancelled by patient user {UserId}", id, currentUserId);
			return _mapper.Map<AppointmentReadOnlyDTO>(appointment);
		}
	}
}