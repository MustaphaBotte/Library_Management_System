CREATE OR ALTER PROCEDURE SP_InsertMember
    @PersonID           INT,
    @Username           NVARCHAR(20),
    @PasswordHash       NVARCHAR(255),
    @PasswordSalt       NVARCHAR(255),
    @LibraryID          INT,
	@Notes              Nvarchar(500),
	@ExpiredAt          DateTime,
    @InsertedID         INT OUTPUT
AS
BEGIN
    BEGIN TRY
        INSERT INTO Members(
            PersonID,
            Username,
            PasswordHash,
            PasswordSalt,
            LibraryID,
			Notes,
			ExpiredAt
        )
        VALUES (
            @PersonID    ,
            @Username    ,
            @PasswordHash,
            @PasswordSalt,
            @LibraryID   ,
	        @Notes       ,
	        @ExpiredAt   
        );

        SET @InsertedID = SCOPE_IDENTITY();
    END TRY
    BEGIN CATCH
        EXEC SP_SaveErrors;
        THROW;
    END CATCH
END