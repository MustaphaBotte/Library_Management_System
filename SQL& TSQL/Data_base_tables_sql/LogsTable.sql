create table Error_Logs 
( LogID int primary key identity,
  ErrorCode int not null,
  ErrorText varchar(max) not null,
  ProcedureSource varchar(100) null,
  ErrorState int null,
  ErrorSeverity int null
);

go
create  procedure SP_SaveErrors
as
begin
begin try

 insert into Error_Logs values(ERROR_NUMBER(),ERROR_MESSAGE(),ERROR_PROCEDURE(),ERROR_STATE(),ERROR_SEVERITY());

end try

begin catch
  -- nothing to do , the try catch just to prevent sending any errors to the app
end catch

end



