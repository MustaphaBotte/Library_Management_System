CREATE view Members_View as 

select * from (
select 
People.PersonID,
MemberID,
CONCAT(People.FirstName ,' ',People.SecondName) as FullName,
People.Email,
People.PhoneNumber,
People.Gender,
Countries.CountryName,
DATEDIFF(DAY,People.DateOfBirth,getdate())/365 as Age,
Username,
JoinedAt,
ExpiredAt,
IsBanned,
LastBorrowAt,
Notes,
GeneralStatus.StatusName,
People.ProfilePicturePath

from Members
inner join People on People.PersonID = Members.PersonID
inner join GeneralStatus on GeneralStatus.StatusID = Members.MembershipStatusID
inner join Countries on People.CountryID = Countries.CountryID
)R1 


select * from Members_View
