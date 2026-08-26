using AutoMapper;
using ClinicBookingApi.Core;
using ClinicBookingApi.DTO;
using ClinicBookingApi.Exceptions;
using ClinicBookingApi.Models;
using ClinicBookingApi.Repositories;
using ClinicBookingApi.Security;

namespace ClinicBookingApi.Services
{
	public class PatientService : IPatientService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly IEncryptionUtil _encryptionUtil;
		private readonly ILogger<PatientService> _logger;

		public PatientService(IUnitOfWork unitOfWork, IMapper mapper,
			ILogger<PatientService> logger, IEncryptionUtil encryptionUtil)
		{
			_encryptionUtil = encryptionUtil;
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_logger = logger;
		}

		public async Task<UserReadOnlyDTO> SignUpUserAsync(PatientSignupDTO request)
		{
			var patient = _mapper.Map<Patient>(request);
			var user = _mapper.Map<User>(request);

			var existingUser = await _unitOfWork.UserRepository.GetUserByUsernameAsync(user.Username);

			if (existingUser != null)
			{
				throw new EntityAlreadyExistsException("User", $"User with username {existingUser.Username} already exists");
			}

			user.Patient = patient;
			user.Password = _encryptionUtil.Encrypt(user.Password);
			await _unitOfWork.UserRepository.AddAsync(user);

			await _unitOfWork.SaveAsync();
			_logger.LogInformation("Patient {Username} signed up successfully.", user.Username);
			return _mapper.Map<UserReadOnlyDTO>(user);
		}

		public async Task<PatientReadOnlyDTO> GetPatientByIdAsync(int id)
		{
			var patient = await _unitOfWork.PatientRepository.GetByIdWithUserAsync(id);
			if (patient == null)
			{
				throw new EntityNotFoundException("Patient", $"Patient with id {id} not found");
			}

			_logger.LogInformation("Patient with id {Id} found", id);
			return _mapper.Map<PatientReadOnlyDTO>(patient);
		}

		public async Task<PaginatedResult<PatientReadOnlyDTO>> GetPaginatedPatientsAsync(int pageNumber, int pageSize)
		{
			var result = await _unitOfWork.PatientRepository.GetPaginatedPatientsWithUserAsync(pageNumber, pageSize);

			var dtoResult = new PaginatedResult<PatientReadOnlyDTO>()
			{
				Data = _mapper.Map<List<PatientReadOnlyDTO>>(result.Data),
				TotalRecords = result.TotalRecords,
				PageNumber = result.PageNumber,
				PageSize = result.PageSize
			};

			_logger.LogInformation("Retrieved {Count} patients", dtoResult.Data.Count);
			return dtoResult;
		}
	}
}