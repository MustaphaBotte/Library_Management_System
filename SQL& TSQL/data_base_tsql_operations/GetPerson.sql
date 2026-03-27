CREATE PROCEDURE SP_GetPersonByID
    @PersonID int
AS
BEGIN
    BEGIN TRY
        SELECT
            PersonID,
            FirstName,
            SecondName,
            Email,
            PhoneNumber,
            DateOfBirth,
            Gender,
            CountryID,
            ProfilePicturePath,
            CreatedAt,
            UpdatedAt,
            CreatedBy
        FROM People
        WHERE PersonID = 28 AND IsDeleted = 0;
    END TRY
    BEGIN CATCH
        EXEC SP_SaveErrors;
        THROW;
    END CATCH
END

exec SP_GetPersonByID 28