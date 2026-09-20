CREATE TABLE People(
PersonID int primary key identity,
FirstName nvarchar(50) not null,
lastName nvarchar(50) not null,
Email nvarchar(255) not null,
PasswordHash varchar(255) not null,
ProfilePicturePath nvarchar(255) null,
CreatedAt datetime2 not null default GETDATE(),
DeletedAt datetime2 null)

CREATE UNIQUE INDEX UQ_People_ActiveEmail ON People(Email) WHERE DeletedAt IS NULL;
CREATE INDEX IDX_People_LastName ON People(LastName);
CREATE INDEX IDX_People_FirstName ON People(FirstName);





CREATE TABLE Members(
MemberID int primary key identity,
PersonID int unique references PEOPLE(PersonID) not null,
MemberShipStatus varchar(15) not null default 'Pending', 
                 CONSTRAINT CK_Members_MembershipStatus 
                 CHECK (MembershipStatus IN ('Active', 'Suspended', 'Expired', 'Pending')))



CREATE TABLE Staff(
StaffID int primary key identity,
PersonID int unique references PEOPLE(PersonID) not null,
Salary Decimal(10,2) check (Salary>0) not null,
Permissions int default 0 CHECK(Permissions>=0) not null)


CREATE TABLE Categories(
CategoryID int primary key identity,
Name nvarchar(100) unique not null,
Description nvarchar(500) null
);


CREATE TABLE Books(
    BookID int primary key identity,
    CategoryID int references Categories(CategoryID) not null,
    ISBN varchar(20) unique not null,   
    Title nvarchar(255) not null,
    Description nvarchar(4000) null,
    PublicationYear smallint null,
    Pages int null,   
    CreatedAt datetime2 not null default GETDATE(),
    DeletedAt datetime2 null,
    CONSTRAINT CK_Books_Pages CHECK (Pages IS NULL OR Pages > 0),
    CONSTRAINT CK_Books_ISBN_NotEmpty CHECK (LTRIM(RTRIM(ISBN)) <> ''),
    CONSTRAINT CK_Books_PublicationYear CHECK (PublicationYear IS NULL OR (PublicationYear > 0 AND PublicationYear <= YEAR(GETDATE()) + 1))
);
CREATE INDEX IDX_Books_CategoryID ON Books(CategoryID);
CREATE INDEX IDX_Books_Title ON Books(Title);



CREATE TABLE Authors(
    AuthorID int primary key identity, 
    FirstName nvarchar(50) not null,
    LastName nvarchar(50) not null,   
    Bio nvarchar(4000) null, 
    PicturePath nvarchar(255) null,
    CreatedAt datetime2 not null default GETDATE(),
	DeletedAt datetime2 null,
);
CREATE INDEX IX_Authors_FirstName ON Authors(FirstName);
CREATE INDEX IX_Authors_LastName ON Authors(LastName);


CREATE TABLE BookAuthors(
    BookID int references Books(BookID) not null,
    AuthorID int references Authors(AuthorID) not null,
    CONSTRAINT PK_BookAuthors PRIMARY KEY (BookID, AuthorID)
);
-- PERFORMANCE NOTE: 
-- filtering by BookID are already lightning fast.
-- However, to find "All books written by Author X", we need the reverse index:
CREATE NONCLUSTERED INDEX IX_BookAuthors_AuthorID ON BookAuthors(AuthorID);

CREATE TABLE BookCopies(
    CopyID int primary key identity, 
    BookID int references Books(BookID) not null,
    
    Status varchar(20) not null default 'Available',
    Condition nvarchar(100) null,
    
    CreatedAt datetime2 not null default GETDATE(),
    DeletedAt datetime2 null,

    
    CONSTRAINT CK_BookCopies_Status 
    CHECK (Status IN ('Available', 'Borrowed','Reserved'))
);
CREATE NONCLUSTERED INDEX IX_BookCopies_BookID ON BookCopies(BookID);




CREATE TABLE Reservations(
    ReservationID int primary key identity,
    BookID int references Books(BookID) not null,
    MemberID int references Members(MemberID) not null,    
    ReservationDate datetime2 not null default GETDATE(),   
    MaxPickUpDate datetime2 not null,    
    Status varchar(20) not null default 'ReadyForPickup',    
    CreatedAt datetime2 not null default GETDATE(),
    CONSTRAINT CK_Reservations_Status 
    CHECK (Status IN ('ReadyForPickup', 'Fulfilled', 'Cancelled', 'Expired')),
    CONSTRAINT CK_Reservations_Dates 
    CHECK ( MaxPickUpDate > ReservationDate)
);

CREATE NONCLUSTERED INDEX IDX_Reservations_BookID ON Reservations(BookID);
CREATE NONCLUSTERED INDEX IDX_Reservations_MemberID ON Reservations(MemberID);



CREATE TABLE Borrows(
    BorrowID int primary key identity,
    BookCopyID int references BookCopies(CopyID) not null, 
	ReservationID int references Reservations(ReservationID) unique not null,
    BorrowDate datetime2 not null default GETDATE(),
    DueDate datetime2 not null,     
    ActualReturnDate datetime2 null, 
    CONSTRAINT CK_Borrows_DueDate CHECK (DueDate > BorrowDate),
    CONSTRAINT CK_Borrows_ActualReturnDate 
    CHECK (ActualReturnDate IS NULL OR ActualReturnDate >= BorrowDate)
);

-- Concurrency Guard: Mathematically guarantees a physical copy cannot be 
-- checked out to two different reservations at the same time
CREATE UNIQUE NONCLUSTERED INDEX UQ_Borrows_ActiveCopy 
ON Borrows(BookCopyID) 
WHERE ActualReturnDate IS NULL;