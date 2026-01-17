CREATE TABLE BookGenre
(
GenreID int not null,
BookID int not null,
Primary key(GenreID,BookID),
FOREIGN KEY (BookID) REFERENCES Books(BookID),
FOREIGN KEY (GenreID) REFERENCES Genres(GenreID)
)