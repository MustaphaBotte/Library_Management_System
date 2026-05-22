CREATE OR ALTER PROCEDURE SP_InsertMember
    @PersonID           INT,
    @LibraryID          INT,
	@Notes              Nvarchar(500),
	@ExpiredAt          DateTime,
    @InsertedID         INT OUTPUT
AS
BEGIN
    BEGIN TRY
        INSERT INTO Members(
            PersonID,        
            LibraryID,
			Notes,
			ExpiredAt
        )
        VALUES (
            @PersonID    ,
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