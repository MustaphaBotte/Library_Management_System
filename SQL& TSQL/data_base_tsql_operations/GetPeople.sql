CREATE PROCEDURE SP_GetPeopleByLastId 
  @LastId int=0,
  @Rows int=100
  as

  set nocount on;
  if @Rows >100 or @Rows<0
    set @Rows = 100;

  if @LastId <0
    set @LastId = 0;

   begin try
      select  top (@Rows) *  from People
      where PersonID>@LastId
      order by PersonID
  end try

  begin catch
            exec SP_SaveErrors;
			throw;
  end catch



  select  top 1 1 from people where PhoneNumber = @PhoneNumber