using ClinicBookingApi.Data;

namespace ClinicBookingApi.Repositories
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly ClinicBookingDbContext _context;
		public IUserRepository UserRepository { get; }
		public IDoctorRepository DoctorRepository { get; }
		public IPatientRepository PatientRepository { get; }
		public IAppointmentRepository AppointmentRepository { get; }

		public UnitOfWork(ClinicBookingDbContext context)
		{
			_context = context;
			UserRepository = new UserRepository(context);
			DoctorRepository = new DoctorRepository(context);
			PatientRepository = new PatientRepository(context);
			AppointmentRepository = new AppointmentRepository(context);
		}

		public async Task<bool> SaveAsync()
		{
			return await _context.SaveChangesAsync() > 0;   // commit & rollback
		}
	}
}