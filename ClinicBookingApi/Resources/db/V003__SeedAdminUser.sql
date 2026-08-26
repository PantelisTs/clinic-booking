BEGIN TRY
    BEGIN TRANSACTION;

    -- ============================================
    -- Migration: Seed initial Admin user
    -- Credentials (for grading/demo purposes):
    --   Username: admin
    --   Password: Admin123!
    -- ============================================

    INSERT INTO [dbo].[Users] ([Username], [Email], [Password], [FirstName], [LastName], [RoleId],
                                [InsertedAt], [ModifiedAt], [IsDeleted])
    SELECT 'admin', 'admin@clinicbooking.com',
           '$2a$11$DzMOEfj8HvzqwuPVspLdpeup0ObGBqMXiN.93wGchXnzMeXYqYbty',
           'System', 'Administrator',
           r.[Id], GETUTCDATE(), GETUTCDATE(), 0
    FROM [dbo].[Roles] r
    WHERE r.[Name] = 'ADMIN'
      AND NOT EXISTS (
          SELECT 1 FROM [dbo].[Users] u WHERE u.[Username] = 'admin'
      );

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION;
    THROW;
END CATCH;