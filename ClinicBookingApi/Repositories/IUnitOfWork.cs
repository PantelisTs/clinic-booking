namespace ClinicBookingApi.Repositories
{
	public interface IUnitOfWork
	{
		IUserRepository UserRepository { get; }
		IDoctorRepository DoctorRepository { get; }
		IPatientRepository PatientRepository { get; }
		IAppointmentRepository AppointmentRepository { get; }

		Task<bool> SaveAsync();
	}
}