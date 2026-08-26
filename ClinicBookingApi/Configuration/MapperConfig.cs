using AutoMapper;
using ClinicBookingApi.DTO;
using ClinicBookingApi.Models;

namespace ClinicBookingApi.Configuration
{
	public class MapperConfig : Profile
	{
		public MapperConfig()
		{
			CreateMap<User, UserReadOnlyDTO>()
				.ForMember(dest => dest.UserRole, opt => opt.MapFrom(src => src.Role.Name));

			CreateMap<DoctorSignupDTO, User>()
				.ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId!.Value));

			CreateMap<DoctorSignupDTO, Doctor>();

			CreateMap<PatientSignupDTO, User>()
				.ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId!.Value));

			CreateMap<PatientSignupDTO, Patient>();

			CreateMap<Doctor, DoctorReadOnlyDTO>()
				.ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.User.FirstName))
				.ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.User.LastName))
				.ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email));
		}
	}
}