CREATE PROCEDURE SP_DeletePerson
    @PersonID int,
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
        SET IsDeleted = 1
        WHERE PersonID = @PersonID;

        SET @IsSuccess = 1;
    END TRY
    BEGIN CATCH
        EXEC SP_SaveErrors;
        THROW;
    END CATCH
END
GO


