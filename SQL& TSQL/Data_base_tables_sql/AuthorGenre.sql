CREATE TABLE AuthorGenre
(
GenreID int not null,
AuthorID int not null,
Primary key(GenreID,AuthorID),
FOREIGN KEY (AuthorID) REFERENCES Authors(AuthorID),
FOREIGN KEY (GenreID) REFERENCES Genres(GenreID)
)