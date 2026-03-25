CREATE OR ALTER PROCEDURE SP_DeleteMember
    @MemberID       INT,
    @IsSuccess      BIT OUTPUT
AS
BEGIN
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM Members WHERE MemberID = @MemberID)
        BEGIN
            SET @IsSuccess = 0;
            RETURN;
        END
		SET @IsSuccess = 1; -- because we have instead on delete
            
			
	    DELETE FROM Members WHERE MemberID = @MemberID;
         
		
		       
    END TRY
    BEGIN CATCH
        EXEC SP_SaveErrors;
        THROW;
    END CATCH
END


select * from Members


