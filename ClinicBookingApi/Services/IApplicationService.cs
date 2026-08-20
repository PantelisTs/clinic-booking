namespace ClinicBookingApi.Services
{
	public interface IApplicationService
	{
		IUserService UserService { get; }
		IDoctorService DoctorService { get; }
		IPatientService PatientService { get; }
	}
}