CREATE TABLE Borrows
(
BorrowID int primary key identity,
ReservationID int not null,
DueDate  datetime not null,
ReturnDate datetime not null,
ActualReturnDate datetime null ,
FOREIGN KEY (ReservationID) REFERENCES Reservations(ReservationID)
)
alter  TABLE Borrows
add constraint Check_for_dates_intergrity
check (ActualReturnDate>DueDate)