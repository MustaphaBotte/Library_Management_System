CREATE DATABASE LMS;
USE LMS;

CREATE TABLE People
(
    PersonID INT IDENTITY(1,1) NOT NULL,
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Email NVARCHAR(255) NOT NULL,
    PasswordHash VARCHAR(255) NOT NULL,
    ProfilePicturePath NVARCHAR(255) NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    DeletedAt DATETIME2 NULL,

    CONSTRAINT PK_People
        PRIMARY KEY (PersonID)
);

CREATE UNIQUE INDEX UQ_People_ActiveEmail ON People(Email) WHERE DeletedAt IS NULL;
CREATE INDEX IDX_People_LastName ON People(LastName);
CREATE INDEX IDX_People_FirstName ON People(FirstName);



CREATE TABLE Members
(
    MemberID INT IDENTITY(1,1) NOT NULL,
    PersonID INT NOT NULL,
    MembershipStatus VARCHAR(15) NOT NULL DEFAULT 'Pending',

    CONSTRAINT PK_Members
        PRIMARY KEY (MemberID),

    CONSTRAINT UQ_Members_Person
        UNIQUE (PersonID),

    CONSTRAINT FK_Members_Person
        FOREIGN KEY (PersonID)
        REFERENCES People(PersonID),

    CONSTRAINT CK_Members_MembershipStatus
        CHECK (MembershipStatus IN
            ('Active', 'Suspended', 'Expired', 'Pending'))
);



CREATE TABLE Staff
(
    StaffID INT IDENTITY(1,1) NOT NULL,
    PersonID INT NOT NULL,
    Salary DECIMAL(10,2) NOT NULL,
    Permissions INT NOT NULL DEFAULT 0,

    CONSTRAINT PK_Staff
        PRIMARY KEY (StaffID),

    CONSTRAINT UQ_Staff_Person
        UNIQUE (PersonID),

    CONSTRAINT FK_Staff_Person
        FOREIGN KEY (PersonID)
        REFERENCES People(PersonID),

    CONSTRAINT CK_Staff_Salary
        CHECK (Salary > 0),

    CONSTRAINT CK_Staff_Permissions
        CHECK (Permissions >= 0)
);


CREATE TABLE Categories
(
    CategoryID INT IDENTITY(1,1) NOT NULL,
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(500) NULL,

    CONSTRAINT PK_Categories
        PRIMARY KEY (CategoryID),

    CONSTRAINT UQ_Categories_Name
        UNIQUE (Name)
);


CREATE TABLE Books
(
    BookID INT IDENTITY(1,1) NOT NULL,
    CategoryID INT NOT NULL,
    ISBN VARCHAR(20) NOT NULL,
    Title NVARCHAR(255) NOT NULL,
    Description NVARCHAR(4000) NULL,
    PublicationYear SMALLINT NULL,
    Pages INT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    DeletedAt DATETIME2 NULL,

    CONSTRAINT PK_Books
        PRIMARY KEY (BookID),

    CONSTRAINT FK_Books_Category
        FOREIGN KEY (CategoryID)
        REFERENCES Categories(CategoryID),

    CONSTRAINT UQ_Books_ISBN
        UNIQUE (ISBN),

    CONSTRAINT CK_Books_Pages
        CHECK (Pages IS NULL OR Pages > 0),

    CONSTRAINT CK_Books_ISBN_NotEmpty
        CHECK (LTRIM(RTRIM(ISBN)) <> ''),

    CONSTRAINT CK_Books_PublicationYear
        CHECK (
            PublicationYear IS NULL
            OR (
                PublicationYear > 0
                AND PublicationYear <= YEAR(GETDATE()) + 1
            )
        )
);

CREATE INDEX IDX_Books_CategoryID ON Books(CategoryID);
CREATE INDEX IDX_Books_Title ON Books(Title);


CREATE TABLE Authors
(
    AuthorID INT IDENTITY(1,1) NOT NULL,
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Bio NVARCHAR(4000) NULL,
    PicturePath NVARCHAR(255) NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    DeletedAt DATETIME2 NULL,

    CONSTRAINT PK_Authors
        PRIMARY KEY (AuthorID)
);

CREATE INDEX IX_Authors_FirstName ON Authors(FirstName);
CREATE INDEX IX_Authors_LastName ON Authors(LastName);


CREATE TABLE BookAuthors
(
    BookID INT NOT NULL,
    AuthorID INT NOT NULL,

    CONSTRAINT PK_BookAuthors
        PRIMARY KEY (BookID, AuthorID),

    CONSTRAINT FK_BookAuthors_Book
        FOREIGN KEY (BookID)
        REFERENCES Books(BookID),

    CONSTRAINT FK_BookAuthors_Author
        FOREIGN KEY (AuthorID)
        REFERENCES Authors(AuthorID)
);

-- PERFORMANCE NOTE: 
-- filtering by BookID is already lightning fast.
-- However, to find "All books written by Author X", we need the reverse index:
CREATE NONCLUSTERED INDEX IX_BookAuthors_AuthorID ON BookAuthors(AuthorID);

CREATE TABLE BookCopies
(
    CopyID INT IDENTITY(1,1) NOT NULL,
    BookID INT NOT NULL,

    Status VARCHAR(20) NOT NULL DEFAULT 'Available',
    Condition NVARCHAR(100) NULL,

    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    DeletedAt DATETIME2 NULL,

    CONSTRAINT PK_BookCopies
        PRIMARY KEY (CopyID),

    CONSTRAINT FK_BookCopies_Book
        FOREIGN KEY (BookID)
        REFERENCES Books(BookID),

    CONSTRAINT CK_BookCopies_Status
        CHECK (Status IN ('Available', 'Borrowed', 'Reserved'))
);
CREATE NONCLUSTERED INDEX IX_BookCopies_BookID ON BookCopies(BookID);




CREATE TABLE Reservations
(
    ReservationID INT IDENTITY(1,1) NOT NULL,
    BookID INT NOT NULL,
    MemberID INT NOT NULL,
    ReservationDate DATETIME2 NOT NULL DEFAULT GETDATE(),
    MaxPickUpDate DATETIME2 NOT NULL,
    Status VARCHAR(20) NOT NULL DEFAULT 'ReadyForPickup',
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),

    CONSTRAINT PK_Reservations
        PRIMARY KEY (ReservationID),

    CONSTRAINT FK_Reservations_Book
        FOREIGN KEY (BookID)
        REFERENCES Books(BookID),

    CONSTRAINT FK_Reservations_Member
        FOREIGN KEY (MemberID)
        REFERENCES Members(MemberID),

    CONSTRAINT CK_Reservations_Status
        CHECK (Status IN
            ('ReadyForPickup', 'Fulfilled', 'Cancelled', 'Expired')),

    CONSTRAINT CK_Reservations_Dates
        CHECK (MaxPickUpDate > ReservationDate)
);

CREATE NONCLUSTERED INDEX IDX_Reservations_BookID ON Reservations(BookID);
CREATE NONCLUSTERED INDEX IDX_Reservations_MemberID ON Reservations(MemberID);



CREATE TABLE Borrows
(
    BorrowID INT IDENTITY(1,1) NOT NULL,
    BookCopyID INT NOT NULL,
    ReservationID INT NOT NULL,
    BorrowDate DATETIME2 NOT NULL DEFAULT GETDATE(),
    DueDate DATETIME2 NOT NULL,
    ActualReturnDate DATETIME2 NULL,

    CONSTRAINT PK_Borrows
        PRIMARY KEY (BorrowID),

    CONSTRAINT FK_Borrows_BookCopy
        FOREIGN KEY (BookCopyID)
        REFERENCES BookCopies(CopyID),

    CONSTRAINT UQ_Borrows_Reservation
        UNIQUE (ReservationID),

    CONSTRAINT FK_Borrows_Reservation
        FOREIGN KEY (ReservationID)
        REFERENCES Reservations(ReservationID),

    CONSTRAINT CK_Borrows_DueDate
        CHECK (DueDate > BorrowDate),

    CONSTRAINT CK_Borrows_ActualReturnDate
        CHECK (
            ActualReturnDate IS NULL
            OR ActualReturnDate >= BorrowDate
        )
);

-- Concurrency Guard: Mathematically guarantees a physical copy cannot be 
-- checked out to two different reservations at the same time
CREATE UNIQUE NONCLUSTERED INDEX UQ_Borrows_ActiveCopy 
ON Borrows(BookCopyID) 
WHERE ActualReturnDate IS NULL;