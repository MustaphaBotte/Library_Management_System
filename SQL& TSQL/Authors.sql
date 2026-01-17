CREATE TABLE Authors(
AuthorID int primary key identity,
ISNI nvarchar(20) not null,
Biography nvarchar(2000) null,
AuthorPicturePath nvarchar(400) null,
Gender char(1) not null check(Gender in ('M','F')),
CountryID int not null,
foreign key(CountryID) references Countries(CountryID),
)
