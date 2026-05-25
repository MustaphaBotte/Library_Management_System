CREATE OR ALTER PROCEDURE SP_UpdateMember
    @MemberID           INT,  
    @ExpiredAt          DATETIME,
    @IsSuccess          BIT OUTPUT
AS
BEGIN
    BEGIN TRY

        IF NOT EXISTS (SELECT 1 FROM Members WHERE MemberID = @MemberID)
        BEGIN
            SET @IsSuccess = 0;
            RETURN;
        END

        UPDATE Members
        SET         
            ExpiredAt     = @ExpiredAt
        WHERE MemberID = @MemberID;

        SET @IsSuccess = 1;
    END TRY
    BEGIN CATCH
        EXEC SP_SaveErrors;
        THROW;
    END CATCH
END