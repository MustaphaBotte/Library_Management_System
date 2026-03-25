CREATE TRIGGER TR_Member_InsteadOfDelete
ON MEMBERS INSTEAD OF DELETE
as
 begin

    SET NOCOUNT ON

    update Members  set IsDeleted = 1 from Members
    inner join Deleted on Deleted.MemberID = Members.MemberID ;
   

    update People  set IsDeleted = 1 from People
    inner join Deleted on Deleted.PersonID = People.PersonID 
   

 end
 