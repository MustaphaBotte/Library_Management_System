
CREATE TABLE Reservations
(
    ReservationID      INT IDENTITY PRIMARY KEY,

    MemberID           INT           NOT NULL,
    BookCopyID         INT           NOT NULL,

    ReservationDate    DATETIME  NOT NULL  DEFAULT GETDATE(),
    PickupDeadline     DATETIME  NOT NULL,
    ReservationStatusID  int  NOT NULL,   -- e.g., 1=Pending, 2=Active, 3=Cancelled, 4=Expired

   
   FOREIGN KEY (MemberID) REFERENCES Members(MemberID), 
   FOREIGN KEY (BookCopyID) REFERENCES BookCopies(BookCopyID)
);
alter table Reservations 
add  FOREIGN KEY (ReservationStatusID) REFERENCES GeneralStatus(StatusID)

