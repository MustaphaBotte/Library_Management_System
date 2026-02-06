CREATE TABLE People
( 
PersonID int primary key identity,
FirstName nvarchar(50) not null,
SecondName nvarchar(50) not null,
Email nvarchar(255) unique,
PhoneNumber nvarchar(15) unique not null,
DateOfBirth date not null,
Gender char(1) not null check(Gender in ('M','F')),
CreatedAt datetime not null default Getdate(),
CreatedBy int not null,
UpdatedAt datetime not null default Getdate(),
CountryID int not null,
ProfilePicturePath nvarchar(500) null,
IsDeleted bit null default 0,
foreign key(CountryID) references Countries(CountryID),
foreign key(CreatedBy ) references Employees(EmployeeID)
)
 
create unique index IX_UniqueNonNullPersonEmail 
on People(email)
where email is not null
						 