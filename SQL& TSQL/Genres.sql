CREATE TABLE Genres
(
GenreID int primary key identity,
GenreName nvarchar(100) unique not null,
Description nvarchar(300) null
)