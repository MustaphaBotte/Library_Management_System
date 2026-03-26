ALTER PROCEDURE SP_UpdatePerson
    @PersonID int,
    @FirstName nvarchar(50),
    @SecondName nvarchar(50),
    @Email nvarchar(255),
    @PhoneNumber nvarchar(15),
    @DateOfBirth date,
    @Gender char(1),
    @CountryID int,
    @ProfilePicturePath nvarchar(500) = NULL,
    @IsSuccess bit OUTPUT
AS
BEGIN
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM People WHERE PersonID = @PersonID AND IsDeleted = 0)
        BEGIN
            SET @IsSuccess = 0;
            RETURN;
        END

        UPDATE People
        SET
            FirstName          = @FirstName,
            SecondName         = @SecondName,
            Email              = @Email,
            PhoneNumber        = @PhoneNumber,
            DateOfBirth        = @DateOfBirth,
            Gender             = @Gender,
            CountryID          = @CountryID,
            ProfilePicturePath = @ProfilePicturePath,
            UpdatedAt          = GETDATE()
        WHERE PersonID = @PersonID AND IsDeleted = 0;

        SET @IsSuccess = 1;-- success
    END TRY
    BEGIN CATCH
        EXEC SP_SaveErrors;
        THROW;
    END CATCH
END


select * from People