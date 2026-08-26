using AutoMapper;
using ClinicBookingApi.Core;
using ClinicBookingApi.DTO;
using ClinicBookingApi.Exceptions;
using ClinicBookingApi.Models;
using ClinicBookingApi.Repositories;
using ClinicBookingApi.Security;

namespace ClinicBookingApi.Services
{
	public class DoctorService : IDoctorService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly IEncryptionUtil _encryptionUtil;
		private readonly ILogger<DoctorService> _logger;

		public DoctorService(IUnitOfWork unitOfWork, IMapper mapper,
			ILogger<DoctorService> logger, IEncryptionUtil encryptionUtil)
		{
			_encryptionUtil = encryptionUtil;
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_logger = logger;
		}

		public async Task<UserReadOnlyDTO> SignUpUserAsync(DoctorSignupDTO request)
		{
			var doctor = _mapper.Map<Doctor>(request);
			var user = _mapper.Map<User>(request);

			var existingUser = await _unitOfWork.UserRepository.GetUserByUsernameAsync(user.Username);

			if (existingUser != null)
			{
				throw new EntityAlreadyExistsException("User", $"User with username {existingUser.Username} already exists");
			}

			user.Doctor = doctor;
			user.Password = _encryptionUtil.Encrypt(user.Password);
			await _unitOfWork.UserRepository.AddAsync(user);

			await _unitOfWork.SaveAsync();
			_logger.LogInformation("Doctor {Username} signed up successfully.", user.Username);
			return _mapper.Map<UserReadOnlyDTO>(user);
		}

		public async Task<DoctorReadOnlyDTO> GetDoctorByIdAsync(int id)
		{
			var doctor = await _unitOfWork.DoctorRepository.GetByIdWithUserAsync(id);
			if (doctor == null)
			{
				throw new EntityNotFoundException("Doctor", $"Doctor with id {id} not found");
			}

			_logger.LogInformation("Doctor with id {Id} found", id);
			return _mapper.Map<DoctorReadOnlyDTO>(doctor);
		}

		public async Task<PaginatedResult<DoctorReadOnlyDTO>> GetPaginatedDoctorsAsync(int pageNumber, int pageSize)
		{
			var result = await _unitOfWork.DoctorRepository.GetPaginatedDoctorsWithUserAsync(pageNumber, pageSize);

			var dtoResult = new PaginatedResult<DoctorReadOnlyDTO>()
			{
				Data = _mapper.Map<List<DoctorReadOnlyDTO>>(result.Data),
				TotalRecords = result.TotalRecords,
				PageNumber = result.PageNumber,
				PageSize = result.PageSize
			};

			_logger.LogInformation("Retrieved {Count} doctors", dtoResult.Data.Count);
			return dtoResult;
		}
	}
}