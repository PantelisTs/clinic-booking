BEGIN TRY
	BEGIN TRANSACTION;
	-- ============================================
	-- ClinicBookingDb - Seed Data
	-- Roles, Capabilities, Role-Capability mappings
	-- ============================================

	-- ============================================
	-- Insert Roles
	-- ============================================
	INSERT INTO [dbo].[Roles] ([Name])
	VALUES
	    ('ADMIN'),
	    ('DOCTOR'),
	    ('PATIENT');

	-- ============================================
	-- Insert Capabilities
	-- ============================================
	INSERT INTO [dbo].[Capabilities] ([Name], [Description])
	VALUES
	    ('INSERT_DOCTOR', 'Create a new doctor'),
	    ('VIEW_DOCTORS', 'View doctor list and details'),
	    ('VIEW_DOCTOR', 'View doctor'),
	    ('EDIT_DOCTOR', 'Modify existing doctor'),
	    ('DELETE_DOCTOR', 'Remove a doctor'),
	    ('VIEW_ONLY_DOCTOR', 'View only own doctor details'),
	    ('INSERT_PATIENT', 'Create a new patient'),
	    ('VIEW_PATIENTS', 'View patient list and details'),
	    ('VIEW_PATIENT', 'View patient'),
	    ('EDIT_PATIENT', 'Modify existing patient'),
	    ('DELETE_PATIENT', 'Remove a patient'),
	    ('VIEW_ONLY_PATIENT', 'View only own patient details'),
	    ('INSERT_APPOINTMENT', 'Create a new appointment'),
	    ('VIEW_APPOINTMENTS', 'View appointment list and details'),
	    ('VIEW_APPOINTMENT', 'View appointment'),
	    ('EDIT_APPOINTMENT', 'Modify existing appointment'),
	    ('DELETE_APPOINTMENT', 'Remove an appointment'),
	    ('CANCEL_APPOINTMENT', 'Cancel own appointment');


	-- ============================================
	-- ADMIN: all capabilities
	-- ============================================
	INSERT INTO [dbo].[RolesCapabilities] ([RolesId], [CapabilitiesId])
	SELECT r.[Id], c.[Id]
	FROM [dbo].[Roles] r
	CROSS JOIN [dbo].[Capabilities] c
	WHERE r.[Name] = 'ADMIN';


	-- ============================================
	-- DOCTOR: VIEW_ONLY_DOCTOR, VIEW_APPOINTMENT,
	--         EDIT_APPOINTMENT
	-- ============================================
	INSERT INTO [dbo].[RolesCapabilities] ([RolesId], [CapabilitiesId])
	SELECT r.[Id], c.[Id]
	FROM [dbo].[Roles] r
	CROSS JOIN [dbo].[Capabilities] c
	WHERE r.[Name] = 'DOCTOR'
	  AND c.[Name] IN ('VIEW_ONLY_DOCTOR', 'VIEW_APPOINTMENT', 'EDIT_APPOINTMENT');


	-- ============================================
	-- PATIENT: VIEW_ONLY_PATIENT, VIEW_DOCTORS,
	--          VIEW_DOCTOR, INSERT_APPOINTMENT,
	--          VIEW_APPOINTMENT, CANCEL_APPOINTMENT
	-- ============================================
	INSERT INTO [dbo].[RolesCapabilities] ([RolesId], [CapabilitiesId])
	SELECT r.[Id], c.[Id]
	FROM [dbo].[Roles] r
	CROSS JOIN [dbo].[Capabilities] c
	WHERE r.[Name] = 'PATIENT'
	  AND c.[Name] IN ('VIEW_ONLY_PATIENT', 'VIEW_DOCTORS', 'VIEW_DOCTOR',
	                    'INSERT_APPOINTMENT', 'VIEW_APPOINTMENT', 'CANCEL_APPOINTMENT');

	COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION;
    THROW;
END CATCH;

DBCC CHECKIDENT ('dbo.Roles', RESEED, 3); -- το επόμενο INSERT θα παράγει 4.
DBCC CHECKIDENT ('dbo.Capabilities', RESEED, 18); -- το επόμενο INSERT θα παράγει 19.
