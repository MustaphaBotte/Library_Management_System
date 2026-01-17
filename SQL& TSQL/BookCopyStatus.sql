create table BookCopyStatus
(StatusID int primary key identity(1,1),
StatusName nvarchar(15) not null unique)

INSERT INTO dbo.BookCopyStatus (StatusName) VALUES
( N'Available'),
( N'CheckedOut'),
( N'Reserved'),
( N'Lost')


