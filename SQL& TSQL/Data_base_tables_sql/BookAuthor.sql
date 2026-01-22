CREATE TABLE BookAuthor
(
AuthorID int not null,
BookID int not null,
Primary key(AuthorID,BookID),
FOREIGN KEY (BookID) REFERENCES Books(BookID),
FOREIGN KEY (AuthorID) REFERENCES Authors(AuthorID)
)