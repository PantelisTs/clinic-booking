namespace ClinicBookingApi.Services
{
	public class ApplicationService : IApplicationService
	{
		public IUserService UserService { get; }
		public IDoctorService DoctorService { get; }
		public IPatientService PatientService { get; }
		public IAppointmentService AppointmentService { get; }

		public ApplicationService(IUserService userService, IDoctorService doctorService,
			IPatientService patientService, IAppointmentService appointmentService)
		{
			UserService = userService;
			DoctorService = doctorService;
			PatientService = patientService;
			AppointmentService = appointmentService;
		}
	}
}