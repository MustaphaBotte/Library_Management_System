
CREATE TABLE BookCopies 
(
    BookCopyID     INT IDENTITY PRIMARY KEY,
    BookID         INT             NOT NULL,   
    PurchasePrice  DECIMAL(10,2)   NULL,
    StatusID         int   NOT NULL, 
    ConditionID      NVARCHAR(50)    NOT NULL,
    Notes          NVARCHAR(100)   NULL,
    FOREIGN KEY (BookID) REFERENCES Books(BookID)
);

ALTER TABLE BookCopies
add foreign key (Status) references BookCopyStatus(StatusID)

ALTER TABLE BookCopies
add foreign key (Condition) references BookCopyCondition(ConditionID)

