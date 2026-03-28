CREATE TABLE Employees
(
EmployeeID int primary key identity,
HireDate date not null,
ContractEndDate date not null,
Salary decimal(5,2) not null ,
IsActive bit not null ,
PersonID int not null,
ManagerID int null,
LibraryID int not null,
foreign key (PersonID) references People(PersonID),
foreign key (ManagerID) references Employees(EmployeeID),
foreign key (LibraryID) references Library(LibraryID)
)