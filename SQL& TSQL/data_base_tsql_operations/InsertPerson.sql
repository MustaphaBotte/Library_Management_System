CREATE PROCEDURE SP_InsertPerson
    @FirstName nvarchar(50),
    @SecondName nvarchar(50),
    @Email nvarchar(255),
    @PhoneNumber nvarchar(15),
    @DateOfBirth date,
	@CreatedAt date,
    @Gender char(1),
    @CountryID int,
    @ProfilePicturePath nvarchar(500) = NULL,
    @CreatedBy int =null,
    @IsDeleted bit = 0,
	@InsertedID int output
AS
BEGIN
begin try
   insert into People(FirstName,SecondName,Email,PhoneNumber,DateOfBirth,Gender,CreatedAt ,CreatedBy,CountryID,ProfilePicturePath,IsDeleted)

   values(@FirstName, @SecondName, @Email,@PhoneNumber,@DateOfBirth, @Gender,@CreatedAt,@CreatedBy,@CountryID,@ProfilePicturePath,@IsDeleted);

   set @InsertedID = SCOPE_IDENTITY();
   return 0 ; --success
end try

begin catch
        exec SP_SaveErrors; 		
        throw ;
		-- send the error back to the application
end catch

END

declare @id int;
exec SP_InsertPerson '','',',',',','2023-10-10','2023-10-10','m',1,'',null,0,@id output;
select  @id;

