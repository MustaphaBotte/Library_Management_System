CREATE TABLE Books (
    BookID            INT IDENTITY PRIMARY KEY,
    ISBN              NVARCHAR(20)     NULL,
    BookTitle         NVARCHAR(200)    NOT NULL,
    BookDescription   NVARCHAR(1000)   NULL,
    CoverImagePath    NVARCHAR(400)    NULL,
    NumberOfPages     INT              NULL,
    PublisherID       INT              NULL, 
    PublicationYear   INT              NULL,  
	FOREIGN KEY (PublisherID) REFERENCES Publishers(PublisherID)
);

create unique index IDX_Book_ISBN on
Books(ISBN) where ISBN is not null

   
