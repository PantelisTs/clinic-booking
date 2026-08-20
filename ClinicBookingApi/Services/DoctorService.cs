using AutoMapper;
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
	}
}