CREATE OR ALTER PROCEDURE SP_GetMemberByID
    @MemberID INT
AS
BEGIN
    BEGIN TRY
        SELECT
            MemberID,
            PersonID,
            Username,
            PasswordHash,
            PasswordSalt,
            JoinedAt,
            ExpiredAt,
            IsBanned,
            MembershipStatusID,
            LastBorrowAt,
            LibraryID,
            Notes
        FROM Members
        WHERE MemberID = @MemberID AND IsDeleted = 0;
    END TRY
    BEGIN CATCH
        EXEC SP_SaveErrors;
        THROW;
    END CATCH
END

EXEC SP_GetMemberByID 1;