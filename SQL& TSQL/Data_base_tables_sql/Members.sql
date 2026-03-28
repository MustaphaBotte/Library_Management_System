
CREATE TABLE Members
(
    MemberID INT IDENTITY(1,1) PRIMARY KEY,       
    PersonID INT NOT NULL,                       
    Username NVARCHAR(50) UNIQUE not null,        
    PasswordHash NVARCHAR(255) NOT NULL,         
    PasswordSalt NVARCHAR(255) NOT NULL,       
    JoinedAt DATETIME NOT NULL DEFAULT Getdate(),
    ExpiredAt DATETIME NOT NULL,                 
    IsBanned BIT NOT NULL DEFAULT 0,             
    MembershipStatusID int NOT NULL, -- e.g., 1=Pending, 2=Active, 3=Cancelled, 4=Expired
    LastBorrowAt Datetime null,
	LibraryID int not null,
	Notes nvarchar(500) null, 
	foreign key (PersonID) references People(PersonID),
    foreign key (LibraryID) references Library(LibraryID)
)

ALTER TABLE Members
add foreign key (MembershipStatusID) references GeneralStatus(statusID)