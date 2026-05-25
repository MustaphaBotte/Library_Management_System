CREATE OR ALTER PROCEDURE SP_InsertMember
    @PersonID           INT,
    @LibraryID          INT,
	@ExpiredAt          DateTime,
    @InsertedID         INT OUTPUT
AS
BEGIN
    BEGIN TRY
        INSERT INTO Members(
            PersonID,        
            LibraryID,
			ExpiredAt
        )
        VALUES (
            @PersonID    ,
            @LibraryID   ,
	        @ExpiredAt   
        );

        SET @InsertedID = SCOPE_IDENTITY();
    END TRY
    BEGIN CATCH
        EXEC SP_SaveErrors;
        THROW;
    END CATCH
END